using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskManage.Services;

namespace TaskManage.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public void About()
    {
      TaskRunner runner = new TaskRunner();
      runner.Run("ping msdev.cc", HttpContext);
    }

    public async Task SocketAsync()
    {

      if (HttpContext.WebSockets.IsWebSocketRequest)
      {
        WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

      }
    }
    private async Task Echo(WebSocket webSocket,object message)
    {
      var buffer = new byte[1024 * 4];
      WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
      while (!result.CloseStatus.HasValue)
      {

        var reply = JsonConvert.SerializeObject(message);
        await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(reply), 0, reply.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
      }
      await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
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
