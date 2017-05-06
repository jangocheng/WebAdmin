using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManage.Services;

namespace TaskManage.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult About()
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
          if (msg == null) continue;

          runner.Run("");
          result = await webSocket.ReceiveAsync(
            new ArraySegment<Byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

        //await Echo(webSocket, "1231");
      }
    }

    public IActionResult Contact()
    {
      ViewData["Message"] = "Your contact page.";

      return View();
    }

    public IActionResult Error()
    {
      return View();
    }
  }
}
