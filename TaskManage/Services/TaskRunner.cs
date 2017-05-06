using System;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TaskManage.Services
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

    public void Run(String command)
    {
      Process myProcess = new Process();
      command = String.IsNullOrEmpty(command) ? "ls" : command;
      try
      {
        myProcess.StartInfo.UseShellExecute = false;
        //linux
        //myProcess.StartInfo.FileName = "bash";
        //myProcess.StartInfo.Arguments = "-c \"" + command + "\"";

        //windows
        myProcess.StartInfo.FileName = "powershell.exe";
        myProcess.StartInfo.Arguments = command;

        myProcess.StartInfo.CreateNoWindow = false;
        myProcess.StartInfo.RedirectStandardOutput = true;
        myProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;

        myProcess.Start();

        StreamReader reader = myProcess.StandardOutput;
        String line = reader.ReadLine();

        while (line != null)
        {
          Echo(line);

          Console.WriteLine(line);
          line = reader.ReadLine();
        }

        Echo("CLOSE_WEBSOCKET");
        myProcess.WaitForExit();
        myProcess.Dispose();

      } catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
    private async void Echo(Object message)
    {
      String reply = JsonConvert.SerializeObject(message);

      if (message.ToString().Equals("CLOSE_WEBSOCKET"))
      {
        await _webSocket.SendAsync(
          new ArraySegment<Byte>(Encoding.UTF8.GetBytes(reply), 0, reply.Length), WebSocketMessageType.Text, true, CancellationToken.None);
      }


      await _webSocket.SendAsync(
        new ArraySegment<Byte>(Encoding.UTF8.GetBytes(reply), 0, reply.Length), WebSocketMessageType.Text, true, CancellationToken.None);
    }
  }
}
