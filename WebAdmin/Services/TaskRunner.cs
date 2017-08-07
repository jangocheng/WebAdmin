using MSDev.Task.Entities;
using MSDev.Task.Helpers;
using MSDev.Task.Tasks;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSDev.DB.Entities;
using static System.String;
using Microsoft.Extensions.Configuration;

namespace WebAdmin.Services
{
	/// <summary>
	/// Use with Websocket
	/// </summary>
	public class TaskRunner
	{
		private readonly WebSocket _webSocket;

		public TaskRunner(WebSocket webSocket)
		{
			_webSocket = webSocket;
		}

		public async Task Run(string command)
		{

			Console.WriteLine("command is :" + command);
			try
			{
				if (command.Equals("bingnews"))
				{
                   
                    var task = new BingNewsTask();
					List<BingNews> bingNewsList = await task.GetNews("微软");

					Console.WriteLine(bingNewsList.Count);
					foreach (BingNews bingNews in bingNewsList)
					{
						await Echo(bingNews.Title);
					}
					await Echo("Done");
				}
				if (command.Equals("c9article"))
				{
					var task = new Channel9Task();
					//获取最近5页articles
					for (int i = 5; i >= 1; i--)
					{
						List<C9Articles> articles = await task.SaveArticles(i);
						if (articles == null) continue;
						foreach (C9Articles c9Article in articles)
						{
							await Echo("article:" + c9Article?.Title);
						}
					}
					// 更新视频页内容
					var videos = await task.SaveVideosAsync(0, 60);
					if (videos != null)
					{
						foreach (C9Videos video in videos)
						{
							await Echo("video:" + video?.Title);
						}
					}
					await Echo("Done");
				}
				if (command.Equals("mvavideos"))
				{
					var task = new MvaTask();
					List<MvaVideos> re = await task.SaveMvaVideo();
					foreach (MvaVideos video in re)
					{
                        var newDetails = task.getMvaDetailAsync(video).Result;
                        await Echo("video:" + video?.Title);
                        await Echo("\t包括子视频：" + newDetails.Count + "个");
                    }
                    await Echo("Done");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		private async Task Echo(string message)
		{
			if (!IsNullOrEmpty(message))
			{
				var bytes = Encoding.UTF8.GetBytes(message);
				await _webSocket.SendAsync(
				  new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
			}

		}
	}
}
