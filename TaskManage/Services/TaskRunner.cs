using System;
using System.Collections.Generic;
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

    private readonly Dictionary<String, String> _taskMap = new Dictionary<String, String>();

    public TaskRunner(WebSocket webSocket)
    {
      _webSocket = webSocket;
      _taskMap.Add("bingnews", "cd /var/task/queue;sudo dotnet MSDev.Taskschd.dll");
    }

    public async Task Run(String command)
    {
      Console.WriteLine(command);
      Process myProcess = new Process();
      command = _taskMap.ContainsKey(command) ? _taskMap[command] : "ls";
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
          await Echo(line);
          Console.WriteLine(line);
          line = reader.ReadLine();
        }

        myProcess.WaitForExit();
        myProcess.Dispose();

      } catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
    private async Task Echo(String message)
    {
      await _webSocket.SendAsync(
        new ArraySegment<Byte>(Encoding.UTF8.GetBytes(message), 0, message.Length), WebSocketMessageType.Text, true, CancellationToken.None);
    }
  }
}
