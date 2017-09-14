using MSDev.DB.Entities;
using MSDev.Work.Helpers;
using MSDev.Work.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSDev.Work.Tasks
{
    public class MvaTask : MSDTask
    {
        private readonly MvaHelper _helper;
        public MvaTask()
        {
            _helper = new MvaHelper();
        }

        /// <summary>
        /// 通过API接口全部获取
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Start()
        {
            int totalNum = await _helper.GetTotalNumberAsync();

            for (int i = 0; i < totalNum; i = i + 100)
            {
                Console.WriteLine($"start:{i}");
                List<MvaVideos> mvaList = await _helper.GetMvaVideos(i, 100);
                Context.MvaVideos.AddRange(mvaList);
                try
                {
                    Context.SaveChanges();
                }
                catch (Exception e)
                {
                    foreach (MvaVideos video in mvaList)
                    {
                        Log.Write("MvaError.txt", JsonConvert.SerializeObject(video));
                    }
                    Console.WriteLine(e.Source + e.Message);
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// 日常更新Mva
        /// </summary>
        /// <returns></returns>
        public async Task<List<MvaVideos>> SaveMvaVideo()
        {
            var lastVideo = Context.MvaVideos
                .OrderByDescending(m => m.CreatedTime)
                .Skip(0).Take(30)
                .ToList();
            List<MvaVideos> mvaList = await _helper.GetMvaVideos(0, 30);

            var toBeAddMcList = mvaList.FindAll(NotSame);
            //不重复内容
            bool NotSame(MvaVideos video)
            {
                if (lastVideo.Any(m => m.Title.Equals(video.Title)))
                {
                    Console.WriteLine("重复:" + video.Title);
                    return false;
                }
                return true;
            }
            try
            {
                Context.MvaVideos.AddRange(toBeAddMcList);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Write("mvaErrors.txt", "MvaTask:SaveMvaVideo=>" + e.Source + e.Message + e.InnerException.Message, true);
                Console.WriteLine(e.Source + e.Message);
            }
            return toBeAddMcList;
        }

        /// <summary>
        /// 获取并存储视频详细内容
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public async Task<List<MvaDetails>> GetMvaDetailAsync(MvaVideos video)
        {
            MvaHelper helper = new MvaHelper();
            var re = await helper.GetMvaDetails(video);
            if (re.Item2.Count > 0)
            {
                try
                {
                    var toBeAddDetail = new List<MvaDetails>();
                    foreach (var item in re.Item2)
                    {
                        //去重处理
                        var exist = Context.MvaDetails.Any(m => m.MvaId.Equals(item.MvaId));
                        if (!exist) toBeAddDetail.Add(item);
                    }
                    if (toBeAddDetail.Count > 0)
                    {
                        Context.MvaDetails.AddRange(toBeAddDetail);
                        Context.SaveChanges();
                    }
                    return toBeAddDetail;
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Source + e.Message + e.InnerException);
                    return default;
                }
            }
            return default;
        }

        /// <summary>
        /// 手动更新最近视频 
        /// </summary>
        /// <returns></returns>
        public async Task<List<MvaVideos>> UpdateRecentDetailAsync()
        {
            //查询需要获取详情的mva视频
            var list = Context.MvaVideos
                .OrderByDescending(m => m.UpdatedTime)
                .Where(m => m.LanguageCode.Equals("zh-cn"))
                .Take(10)
                .ToList();
            var updateVideos = new List<MvaVideos>();
            foreach (var item in list)
            {
                var re = await GetMvaDetailAsync(item);
                if (re.Count > 0)
                {
                    updateVideos.Add(item);
                }
            }
            return updateVideos;
        }

        /// <summary>
        /// 批量更新视频详细信息
        /// </summary>
        /// <param name="num">要更新的数量</param>
        /// <returns></returns>
        public List<MvaVideos> UpdateDetail(int num = 999)
        {
            //查询需要获取详情的mva视频
            var list = Context.MvaVideos
                .OrderByDescending(m => m.UpdatedTime)
                .Where(m => m.LanguageCode.Equals("zh-cn"))
                .Take(num)
                .ToList();

            var beUpdateList = new List<MvaVideos>(list);

            MvaHelper helper = new MvaHelper();
            Console.WriteLine($"共有{list.Count}条数据需要处理");
            int i = 1;
            Parallel.ForEach(beUpdateList, new ParallelOptions { MaxDegreeOfParallelism = 10 }, item =>
            {
                mvaDetailTask(item);
            });

            Console.WriteLine("开始写入详情数据");
            Context.SaveChanges();
            Console.WriteLine("开始更新video detailDescription");
            foreach (var item in beUpdateList)
            {
                var oldvideo = Context.MvaVideos.Find(item.Id);
                oldvideo.DetailDescription = item.DetailDescription;
                Context.MvaVideos.Update(oldvideo);
            }
            Context.SaveChanges();
            Console.WriteLine("写入数据成功");

            //内部方法
            void mvaDetailTask(MvaVideos item)
            {
                var re = helper.GetMvaDetails(item).Result;
                //更新mvavideo表
                var detailDescription = re.Item1;
                item.DetailDescription = detailDescription;
                //Context.Update(item);

                //插入mvadetail
                var mvaDetails = re.Item2;
                if (mvaDetails != null)
                {
                    Context.MvaDetails.AddRange(mvaDetails);

                }
                Console.WriteLine($"获取第{i}条数据");
                i++;
            }
            return beUpdateList;
        }

    }
}
