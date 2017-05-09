using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using MSDev.Task.Helpers;
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

      Services.AddTransient(typeof(ApiHelper));
      Services.AddScoped(typeof(BingNewsTask));
      Services.AddSingleton(factory);

      ////确保数据库建立
      //var dbContext = new AppDbContext();
      //dbContext.Database.Migrate();
      //Console.ReadLine();

      if (Run("bingnews").Result)
      {
        Console.WriteLine("BingNews finish!");
      }
    }

    /// <summary>
    /// 运行服务
    /// </summary>
    /// <param name="serviceName">服务名称</param>
    public static async Task<Boolean> Run(String serviceName)
    {
      serviceName = serviceName.ToLower();
      switch (serviceName)
      {
        case "bingnews":
          BingNewsTask task = Services.BuildServiceProvider().GetService<BingNewsTask>();
          await task.GetNews("微软");
          break;
        default:
          break;
      }
      return true;
    }
  }
}