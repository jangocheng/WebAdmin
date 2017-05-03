using System;
using System.Net;
using System.Net.Sockets;

namespace TestSocket
{
  public class Program
  {
    public static void Main(string[] args)
    {

      StartServer();
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
          Console.Write("Waiting for a connection... ");

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
            Console.WriteLine("Sent: {0}", data);
          }

          //client.Dispose();
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