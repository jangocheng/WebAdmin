using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TaskManage.Services
{
  public class TaskRunner
  {


    public TaskRunner()
    {

    }

    public void Run(string command, HttpContext context)
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
        string line = reader.ReadLine();

        while (line != null)
        {
          context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(line+"\r\n"),0,line.Length);
          context.Response.Body.FlushAsync();
          
          Console.WriteLine(line);
          line = reader.ReadLine();
        }

        myProcess.WaitForExit();
        myProcess.Dispose();
        // This code assumes the process you are starting will terminate itself. 
        // Given that is is started without a window so you cannot terminate it 
        // on the desktop, it must terminate itself or you can do it programmatically
        // from this application using the Kill method.
      } catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

  }
}
