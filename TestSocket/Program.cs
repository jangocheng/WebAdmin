using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TestSocket
{
  public class Program
  {
    public static void Main(string[] args)
    {

      Process myProcess = new Process();
      try
      {
        myProcess.StartInfo.UseShellExecute = false;
        // You can start any process, HelloWorld is a do-nothing example.
        myProcess.StartInfo.FileName = "powershell.exe";
        myProcess.StartInfo.Arguments = @"dotnet C:\Users\zpty\Source\Repos\github\TaskQueue\TestClient\bin\Debug\netcoreapp1.1\TestClient.dll";
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.StartInfo.CreateNoWindow = true;
        myProcess.StartInfo.RedirectStandardOutput = true;
        myProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;

        myProcess.Start();

        StreamReader reader = myProcess.StandardOutput;
        string line = reader.ReadLine();
       
        while (line!=null)
        {
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

    static void StartServer()
    {
      TcpListener server = null;
      try
      {
        Int32 port = 13000;
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");

        server = new TcpListener(localAddr, port);

        server.Start();

        Byte[] bytes = new Byte[256];
        String data = null;

        while (true)
        {
          Console.WriteLine("Waiting for a connection... ");

          TcpClient client = server.AcceptTcpClientAsync().Result;
          Console.WriteLine("Connected!");

          data = null;

          NetworkStream stream = client.GetStream();

          int i;

          while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
          {
            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine("Received: {0}", data);

            data = data.ToUpper();

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(msg, 0, msg.Length);
          }

          client.Dispose();
        }
      } catch (SocketException e)
      {
        Console.WriteLine("SocketException: {0}", e);
      } finally
      {
        server.Stop();
      }


      Console.WriteLine("\nHit enter to continue...");
      Console.Read();
    }
  }
}