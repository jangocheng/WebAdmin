using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSDev.DB.Entities;
using MSDev.Task.Helpers;
using MSDev.Task.Tools;
using Newtonsoft.Json;
using static System.String;

namespace MSDev.Task.Tasks
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
			var toBeAddMcList = mvaList.ToList();

			foreach (MvaVideos mvaVideo in mvaList)
			{
				foreach (MvaVideos video in lastVideo)
				{
					if (video.Title == mvaVideo.Title)
					{
						Console.WriteLine("repeat:" + mvaVideo.Title);
						toBeAddMcList.Remove(mvaVideo);
						break;
					}
				}
			}
            try
            {
                Context.MvaVideos.AddRange(toBeAddMcList);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source+e.Message);
            }
			return toBeAddMcList;
		}


        /// <summary>
        /// 更新视频详细信息
        /// </summary>
        public void UpdateDetail(string url)
        {

        }

	}
}
