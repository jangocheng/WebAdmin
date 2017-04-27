using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using MSDev.DataAgent;
using MSDev.Taskschd.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using MSDev.DataAgent.Repositories;
using MSDev.DataAgent.Agents;
using System.IO;

namespace MSDev.Taskschd
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
          options.UseSqlServer(config.GetConnectionString("localSqlServer")));

      services.AddScoped<IBingNewsRepository, BingNewsAgent>();
      services.AddScoped<BingNewsTask>();

      ////确保数据库建立
      //var dbContext = new AppDbContext();
      //dbContext.Database.Migrate();

      //Console.ReadLine();
      Run("bingnews");
      Thread.Sleep(1000 * 10);
    }

    /// <summary>
    /// 运行服务
    /// </summary>
    /// <param name="serviceName">服务名称</param>
    public static async void Run(string serviceName)
    {
      serviceName = serviceName.ToLower();
      switch (serviceName)
      {
        case "bingnews":
          var task = services.BuildServiceProvider().GetService<BingNewsTask>();
          await task.GetNews("微软");
          Console.WriteLine("Done");
          break;
        default:
          break;
      }

    }
  }
}