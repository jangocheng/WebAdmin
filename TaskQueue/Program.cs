using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using MSDev.DataAgent;
using MsDev.Taskschd.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using MSDev.DataAgent.Agents.Interfaces;
using MSDev.DataAgent.Agents.News;

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
            var config = new ConfigurationBuilder().AddJsonFile("config.json").Build();

            services.AddLogging();

            ILoggerFactory factory = new LoggerFactory();
            factory.AddConsole();

            services.AddSingleton(factory);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IBingNewsAgent, BingNewsAgent>();
            services.AddScoped<BingNewsTask>();

            ////确保数据库建立
            //var dbContext = new AppDbContext();
            //dbContext.Database.Migrate();

            //Console.ReadLine();
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