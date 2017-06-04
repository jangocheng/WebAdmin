using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MSDev.Task.Tasks;

namespace WebAdmin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var task = new Channel9Task();
            task.Start();
            Console.ReadLine();



            //IWebHost host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .UseApplicationInsights()
            //    .Build();
            //host.Run();
        }


    }
}
