using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using MSDev.DB;
using MSDev.Task.Helpers;
using MSDev.Task.Tasks;
using Newtonsoft.Json;


namespace MSDev.Task
{
	public static class Program
	{
		private static readonly IServiceCollection Services = new ServiceCollection();
		public static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			// 加载配置文件
			IConfigurationBuilder builder = new ConfigurationBuilder()
				  .SetBasePath(Directory.GetCurrentDirectory())
				  .AddJsonFile("config.json")
				  .AddJsonFile($"config.Development.json");

			IConfigurationRoot config = builder.Build();

			Services.AddLogging();

			ILoggerFactory factory = new LoggerFactory();
			factory.AddConsole();

			Services.AddTransient(typeof(ApiHelper));
			Services.AddScoped(typeof(BingNewsTask));
			Services.AddScoped(typeof(DevBlogsTask));
			Services.AddScoped(typeof(Channel9Task));

			Services.AddDbContext<AppDbContext>(
				option => option.UseSqlServer(
					config.GetConnectionString("DefaultConnection")
				)
			);

			Services.AddSingleton(factory);
			//var task = GetService<Channel9Task>();
			//task.Start();

			Console.ReadLine();

		}


		public static T GetService<T>()
		{
			return Services.BuildServiceProvider().GetService<T>();
		}

	}
}