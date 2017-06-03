using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using System.IO;
using MSDev.Task.Helpers;
using MSDev.Task.Tasks;


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
				  .AddJsonFile("config.json");

			IConfigurationRoot config = builder.Build();

			Services.AddLogging();

			ILoggerFactory factory = new LoggerFactory();
			factory.AddConsole();

			Services.AddTransient(typeof(ApiHelper));
			Services.AddScoped(typeof(BingNewsTask));
			Services.AddScoped(typeof(DevBlogsTask));
			Services.AddScoped(typeof(Channel9Task));


			Services.AddSingleton(factory);

			////确保数据库建立
			//var dbContext = new AppDbContext();
			//dbContext.Database.Migrate();
			//Console.ReadLine();

			//BingNewsTask task = GetService<BingNewsTask>();
			//task.GetNews("微软");

			//DevBlogsTask blogTask = GetService<DevBlogsTask>();

			//if(blogTask.GetNewsAsync().Result){

			//	Console.WriteLine("blogtask done");
			//}

			var task = GetService<Channel9Task>();
			task.Start();

			Console.ReadLine();

		}


		public static T GetService<T>()
		{
			return Services.BuildServiceProvider().GetService<T>();
		}

	}
}