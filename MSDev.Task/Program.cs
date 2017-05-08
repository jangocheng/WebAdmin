using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using MSDev.Task.Tasks;

namespace MSDev.Task
{
  public static class Program
  {
    private static readonly IServiceCollection Services = new ServiceCollection();

    public static void Main(String[] args)
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

      Services.AddSingleton(factory);

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
    public static async void Run(String serviceName)
    {
      serviceName = serviceName.ToLower();
      switch (serviceName)
      {
        case "bingnews":
          BingNewsTask task = Services.BuildServiceProvider().GetService<BingNewsTask>();
          await task.GetNews("微软");
          Console.WriteLine("Done");
          break;
        default:
          break;
      }

    }
  }
}