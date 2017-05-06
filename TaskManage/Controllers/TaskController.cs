using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManage.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManage.Controllers
{
  public class TaskController : Controller
  {
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }

    public async Task RunTask()
    {
      if (HttpContext.WebSockets.IsWebSocketRequest)
      {

        WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        Byte[] buffer = new Byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(
          new ArraySegment<Byte>(buffer), CancellationToken.None);
        TaskRunner runner = new TaskRunner(webSocket);

        while (!result.CloseStatus.HasValue)
        {
          String msg = buffer.ToString();
          if (msg == null)
            continue;
          await runner.Run(msg);
          result = await webSocket.ReceiveAsync(
            new ArraySegment<Byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        //await Echo(webSocket, "1231");
      }
    }
  }
}
