using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSDev.DB.Models;
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
				List<MvaVideo> mvaList = await _helper.GetMvaVideos(i, 100);
				Context.MvaVideos.AddRange(mvaList);
				try
				{
					Context.SaveChanges();
				}
				catch (Exception e)
				{
					foreach (MvaVideo video in mvaList)
					{
						Log.Write("MvaError.txt", JsonConvert.SerializeObject(video));
					}
					Console.WriteLine(e.Source + e.Message);
					throw;
				}
			}
			return true;
		}
	}
}
