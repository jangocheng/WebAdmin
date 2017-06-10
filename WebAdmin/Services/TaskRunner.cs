using MSDev.Task.Entities;
using MSDev.Task.Helpers;
using MSDev.Task.Tasks;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSDev.DB.Models;

namespace WebAdmin.Services
{
	/// <summary>
	/// Use with Websocket
	/// </summary>
	public class TaskRunner
	{
		private readonly WebSocket _webSocket;
		private readonly ApiHelper _apiHelper;
		private WebSocket webSocket;

		public TaskRunner(WebSocket webSocket)
		{
			this.webSocket = webSocket;
		}

		public TaskRunner(WebSocket webSocket, ApiHelper apiHelper)
		{
			_webSocket = webSocket;
			_apiHelper = apiHelper;
		}

		public async Task Run(string command)
		{

			Console.WriteLine("command is :" + command);
			try
			{
				if (command.Equals("bingnews"))
				{
					var task = new BingNewsTask(_apiHelper);
					List<BingNewsEntity> bingNewsList = await task.GetNews("微软");

					foreach (BingNewsEntity bingNews in bingNewsList)
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
						List<C9Article> articles = await task.SaveArticles(i);
						if (articles == null) continue;
						foreach (C9Article c9Article in articles)
						{
							await Echo("article:" + c9Article.Title);
						}
					}
					// 更新视频页内容
					var videos = await task.SaveVideosAsync(0, 60);
					if (videos != null)
					{
						foreach (C9Video video in videos)
						{
							await Echo("video:" + video.Title);
						}
					}
					await Echo("Done");
				}
				if (command.Equals("mvavideos"))
				{
					var task = new MvaTask();
					List<MvaVideo> re=await task.SaveMvaVideo();
					foreach (MvaVideo video in re)
					{
						await Echo("video:" + video.Title);
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

			if (!String.IsNullOrEmpty(message))
			{

				byte[] bytes = Encoding.UTF8.GetBytes(message);
				await _webSocket.SendAsync(
				  new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
			}

		}
	}
}
