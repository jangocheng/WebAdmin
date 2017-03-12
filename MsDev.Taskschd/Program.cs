using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using MSDev.DataAgent;
using MsDev.Taskschd.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using MsDev.DataAgent.Repositories;
using MSDev.DataAgent.Agents;
using System.IO;

namespace MsDev.Taskschd
{
    public static class Program
    {
        private static IServiceCollection services = new ServiceCollection();

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // 加载配置文件
            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("config.json");

            var config = builder.Build();



            services.AddLogging();

            ILoggerFactory factory = new LoggerFactory();
            factory.AddConsole();

            services.AddSingleton(factory);

            //连接字符串使用config.json中自定义的替换
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("cnConnection")));

            services.AddScoped<IBingNewsRepository, BingNewsAgent>();
            services.AddScoped<BingNewsTask>();

            ////确保数据库建立
            //var dbContext = new AppDbContext();
            //dbContext.Database.Migrate();

            //Console.ReadLine();
            Run();
            Thread.Sleep(1000 * 10);
        }

        public static async void Run()
        {
            var task = services.BuildServiceProvider().GetService<BingNewsTask>();
            await task.GetNews("微软");
            Console.WriteLine("Done");
        }
    }
}