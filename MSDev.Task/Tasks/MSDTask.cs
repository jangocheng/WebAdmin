using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;


namespace MSDev.Task.Tasks
{
	public class MSDTask
	{
		private static readonly IServiceCollection Services = new ServiceCollection();

		protected readonly AppDbContext Context;
		public MSDTask()
		{
			StartUp();
			Context = GetService<AppDbContext>();

		}

		public MSDTask(AppDbContext context)
		{
			Context = context;
		}
		public static void StartUp()
		{
			Console.OutputEncoding = Encoding.UTF8;
			// 加载配置文件
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.Development.json");
				  //.AddJsonFile($"appsettings.Production.json");
			IConfigurationRoot config = builder.Build();

			ILoggerFactory factory = new LoggerFactory();
			factory.AddConsole();

			Services.AddLogging();
			Services.AddSingleton(factory);

			Services.AddDbContext<AppDbContext>(
				option => option.UseSqlServer(
					config.GetConnectionString("DefaultConnection")
				)
			);
		}

		public static T GetService<T>()
		{
			return Services.BuildServiceProvider().GetService<T>();
		}

	}
}