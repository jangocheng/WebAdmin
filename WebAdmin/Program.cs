using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MSDev.DB.Entities;
using MSDev.Work.Tasks;
using MSDev.Work.Tools;
using System.Text;
using Microsoft.AspNetCore;

namespace WebAdmin
{
    public class Program
    {
        private static bool IsTask = false;//是否运行Task
                                           //private static bool IsTask = false;

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
        }
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var task = new C9EvnentTask();
            await task.GetVideoDetailAsync();

            if (IsTask)
            {
                var t = new Thread(StartAutoTask)
                {
                    IsBackground = true
                };
                t.Start();
            }
            BuildWebHost(args).Run();

        }

        /// <summary>
        /// 自动执行任务
        /// </summary>
        public static void StartAutoTask()
        {
            while (true)
            {
                int hour = DateTime.Now.ToLocalTime().Hour;
                if (hour != 7 && hour != 12 && hour != 18)
                {
                    Thread.Sleep(60 * 60 * 1000);
                    continue;
                };
                string fileName = "./AutoTask/Task-" + DateTime.Now.ToLocalTime().Date.ToString("yyyy-MM-dd") + ".txt";

                try
                {
                    Console.WriteLine(hour+": Task start ");
                    Log.Write(fileName, $"{hour}点 采集内容：");

                    Log.Write(fileName, "rssNews Start!");
                    var task3 = new DevBlogsTask();
                    var rssnews = task3.GetNewsAsync().Result;
                    foreach (var news in rssnews)
                    {
                        Log.Write(fileName, "\t" + news?.Title);
                    }
                    Log.Write(fileName, "rssNews End!\n");

                    Log.Write(fileName, "BingNewsTask Start!");
                    var task = new BingNewsTask();
                    var bingNewsList = task.GetNews("微软").Result;

                    foreach (BingNews bingNews in bingNewsList)
                    {
                        Log.Write(fileName, "\t" + bingNews?.Title);
                    }
                    var bingNewsList1 = task.GetNews("科技").Result;

                    foreach (BingNews bingNews in bingNewsList1)
                    {
                        Log.Write(fileName, "\t" + bingNews?.Title);
                    }
                    Log.Write(fileName, "BingNewsTask End!\n");

                    //TODO: 处理登录
                    Log.Write(fileName, "Channel9Task Start!");
                    var task1 = new Channel9Task();
                    //获取最近5页articles
                    for (int i = 5; i >= 1; i--)
                    {
                        var articles = task1.SaveArticles(i).Result;
                        if (articles == null) continue;
                        foreach (C9Articles c9Article in articles)
                        {
                            Log.Write(fileName, "\t" + c9Article?.Title);
                        }
                    }
                    // 更新视频页内容
                    var videos = task1.SaveVideosAsync(0, 60).Result;
                    if (videos != null)
                    {
                        foreach (C9Videos video in videos)
                        {
                            Log.Write(fileName, "\t" + video?.Title);
                        }
                    }
                    Log.Write(fileName, "Channel9Task End!\n");

                    Log.Write(fileName, "MVATask Start!");
                    var task2 = new MvaTask();
                    var re = task2.SaveMvaVideo().Result;
                    //更新视频 详细内容
                    foreach (MvaVideos video in re)
                    {
                        var newDetails = task2.GetMvaDetailAsync(video).Result;
                        Log.Write(fileName, "\t" + video?.Title);
                        Log.Write(fileName, "\t 包括子视频：" + newDetails.Count + "个");

                    }
                    Log.Write(fileName, "MVATask End!\n");
                    Log.Write(fileName, "=========Finished==========\n");


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Source + e.Message + e.InnerException);
                    Log.Write(fileName, e.Source + e.Message);
                }
                Thread.Sleep(60 * 60 * 1000);
            }
        }
    }
}
