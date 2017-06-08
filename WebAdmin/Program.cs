using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MSDev.Task.Tasks;

namespace WebAdmin
{
	public class Program
	{
		public static void Main(string[] args)
		{

			//IWebHost host = new WebHostBuilder()
			//	.UseKestrel()
			//	.UseContentRoot(Directory.GetCurrentDirectory())
			//	.UseIISIntegration()
			//	.UseStartup<Startup>()
			//	.UseApplicationInsights()
			//	.Build();
			//host.Run();


			var task = new Channel9Task();

			task.SavePageVideosAsync().Wait();

			Console.ReadLine();

		}


	}
}
