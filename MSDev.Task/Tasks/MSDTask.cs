using System;
using System.Collections.Generic;
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

        public static IConfigurationRoot Configuration  { get; set; }

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


		public static List<(string name, string value)> GetTaskEnv()
		{
			var re = new List<(string name, string value)>();

			string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{env}.json", true);
			IConfigurationRoot config = builder.Build();
			re.Add(("环境",env));
			re.Add(("数据库", config.GetConnectionString("DefaultConnection")));

			return re;

		}

		/// <summary>
		/// 读取配置
		/// </summary>
		public static void StartUp()
		{
			Console.OutputEncoding = Encoding.UTF8;

			string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			// 加载配置文件
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env}.json");

			 Configuration = builder.Build();

			ILoggerFactory factory = new LoggerFactory();
			factory.AddConsole();

			Services.AddLogging();
			Services.AddSingleton(factory);

			Services.AddDbContext<AppDbContext>(
				option => option.UseSqlServer(
                    Configuration.GetConnectionString("OnlineConnection")
				)
			);
		}

		public static T GetService<T>()
		{
			return Services.BuildServiceProvider().GetService<T>();
		}

	}
}