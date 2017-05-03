using System;
using System.Net.Sockets;

namespace TestClient
{
  class Program
  {
    static void Main(string[] args)
    {
      ConnectAsync("127.0.0.1", "I'm client");

    }

    static void ConnectAsync(String server, String message)
    {
      try
      {
        Int32 port = 13000;
        TcpClient client = new TcpClient();
        client.ConnectAsync(server, port).Wait();
        NetworkStream stream = client.GetStream();
        while (message != "end")
        {
          Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
          stream.Write(data, 0, data.Length);

          data = new Byte[256];
          String responseData = String.Empty;
          Int32 bytes = stream.Read(data, 0, data.Length);
          responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
          Console.WriteLine("Received: {0}", responseData);

          message = Console.ReadLine();
        }
        stream.Dispose();
        client.Dispose();

      } catch (ArgumentNullException e)
      {
        Console.WriteLine("ArgumentNullException: {0}", e);
      } catch (SocketException e)
      {
        Console.WriteLine("SocketException: {0}", e);
      }
    }
  }
}