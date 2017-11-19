using MSDev.DB.Entities;
using MSDev.Work.Helpers;
using MSDev.Work.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.String;
namespace MSDev.Work.Tasks
{
    public class C9EvnentTask : MSDTask
    {
        private readonly C9Helper _helper;
        public C9EvnentTask()
        {
            _helper = new C9Helper();
        }

        public void DailyUpdate()
        {

        }
        /// <summary>
        /// C9 event 目录信息
        /// </summary>
        /// <returns></returns>
        public async Task GetEeventsAsync()
        {
            var list = await _helper.GetEventsAsync();
            try
            {
                foreach (var item in list)
                {
                    if (Context.C9Event.Any(m => m.TopicName.Equals(item.TopicName))) continue;

                    Context.C9Event.Add(item);
                }
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.Source);
            }
        }

        /// <summary>
        /// 获取EventVideos
        /// </summary>
        public async Task GetVideosAsync()
        {
            var events = Context.C9Event.ToList();
            var allEventVideos = new List<C9Articles>();
            foreach (var item in events)
            {
                var eventVideos = await _helper.GetEventVideosAsync(item);
                if (eventVideos.Count > 0)
                {
                    Console.WriteLine(item.TopicName + JsonConvert.SerializeObject(eventVideos));
                    allEventVideos.AddRange(eventVideos);
                }
            }
            Log.Write("C9EventVideos.json", JsonConvert.SerializeObject(allEventVideos));
            Console.WriteLine("====FINISHED");
        }

        /// <summary>
        /// 获取并保存事件视频详情
        /// </summary>
        public async Task GetVideoDetailAsync()
        {
            //读取数据
            var content = File.ReadAllText("C9EventVideos.json", Encoding.UTF8);
            var eventVideos = JsonConvert.DeserializeObject<List<C9Articles>>(content);
            //eventVideos = eventVideos.Take(20).ToList();
            var allVideoDetail = new ConcurrentBag<EventVideo>();

            int totalNumber = eventVideos.Count;
            Console.WriteLine($"共 {totalNumber} 个视频");

            int i = 1;
            var tasks = new List<Task>();
            foreach (var item in eventVideos)
            {
                int currentIndex = i;
                var task = Task.Run(() => getEventVideoDetailAsync(currentIndex, item));
                tasks.Add(task);
                i++;
            }
            async Task getEventVideoDetailAsync(int currentIndex, C9Articles item)
            {
                Console.WriteLine($"开始获取第 {currentIndex} 个视频");
                var videoDetail = await _helper.GetEventVideoPage(item);
                Console.WriteLine($"获取第 {currentIndex} 个视频完成");
                if (videoDetail != null)
                {
                    allVideoDetail.Add(videoDetail);
                }
            }
            //TODO 去重
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(JsonConvert.SerializeObject(allVideoDetail));
            try
            {
                Console.WriteLine("开始入库");
                Context.EventVideo.AddRange(allVideoDetail);
                await Context.SaveChangesAsync();
                Console.WriteLine("入库完成");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
