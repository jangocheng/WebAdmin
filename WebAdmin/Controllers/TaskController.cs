using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using MSDev.Task.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAdmin.Controllers
{
	public class TaskController : BaseController
	{

		public TaskController(ApiHelper apiHelper) : base(apiHelper)
		{
		}

		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public async Task RunTask()
		{
			if (HttpContext.WebSockets.IsWebSocketRequest)
			{

				WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
				byte[] buffer = new byte[1024 * 4];
				WebSocketReceiveResult result = await webSocket.ReceiveAsync(
				  new ArraySegment<byte>(buffer), CancellationToken.None);
				var runner = new TaskRunner(webSocket,_aipHelper);

				while (!result.CloseStatus.HasValue)
				{

					string msg = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
					if (msg == null)
					{
						continue;
					}

					await runner.Run(msg);
					result = await webSocket.ReceiveAsync(
					  new ArraySegment<byte>(buffer), CancellationToken.None);
				}
				await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
				//await Echo(webSocket, "1231");
			}
		}
	}
}
