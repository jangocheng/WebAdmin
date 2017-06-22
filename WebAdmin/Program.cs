using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MSDev.DB.Models;
using MSDev.Task.Tasks;
using MSDev.Task.Tools;

namespace WebAdmin
{
	public class Program
	{
		public static void Main(string[] args)
		{

			var t = new Thread(StartAutoTask);
			t.Start();

			IWebHost host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.UseApplicationInsights()
				.Build();
			host.Run();
			//var task=new MvaTask();
			//bool result = task.Start().Result;
		}

		/// <summary>
		/// 自动执行任务
		/// </summary>
		public static void StartAutoTask()
		{
			while (true)
			{
				int hour = DateTime.Now.ToLocalTime().Hour;
				if (hour != 8 && hour != 18 && hour != 12) continue;
				string fileName = "./AutoTask/Task-" + DateTime.Now.ToLocalTime().Date.ToString("yyyy-MM-dd") + ".txt";

				Log.Write(fileName, "BingNewsTask Start!");
				var task = new BingNewsTask();
				var bingNewsList = task.GetNews("微软").Result;

				foreach (BingNews bingNews in bingNewsList)
				{
					Log.Write(fileName, "\t" + bingNews.Title);
				}
				Log.Write(fileName, "BingNewsTask End!\n");

				Log.Write(fileName, "Channel9Task Start!");
				var task1 = new Channel9Task();
				//获取最近5页articles
				for (int i = 5; i >= 1; i--)
				{
					var articles = task1.SaveArticles(i).Result;
					if (articles == null) continue;
					foreach (C9Article c9Article in articles)
					{
						Log.Write(fileName, "\t" + c9Article.Title);

					}
				}
				// 更新视频页内容
				var videos = task1.SaveVideosAsync(0, 60).Result;
				if (videos != null)
				{
					foreach (C9Video video in videos)
					{
						Log.Write(fileName, "\t" + video.Title);

					}
				}
				Log.Write(fileName, "Channel9Task End!\n");


				Log.Write(fileName, "MVATask Start!");
				var task2 = new MvaTask();
				var re = task2.SaveMvaVideo().Result;
				foreach (MvaVideo video in re)
				{
					Log.Write(fileName, "\t" + video.Title);
				}
				Log.Write(fileName, "MVATask End!\n");

				Thread.Sleep(60 * 60 * 1000);
			}


		}
	}
}
