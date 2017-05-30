using MSDev.Task.Entities;
using MSDev.Task.Helpers;
using MSDev.Task.Tasks;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
