using MSDev.Work.Tasks;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSDev.DB.Entities;
using static System.String;

namespace WebAdmin.Services
{
    /// <summary>
    /// Use with Websocket
    /// </summary>
    public class TaskRunner
    {
        private readonly WebSocket _webSocket;

        public TaskRunner(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public async Task Run(string command)
        {

            Console.WriteLine("command is :" + command);
            try
            {
                switch (command)
                {

                    case "bingnews":
                        var task = new BingNewsTask();
                        List<BingNews> bingNewsList = await task.GetNews("微软");

                        Console.WriteLine(bingNewsList.Count);
                        foreach (BingNews bingNews in bingNewsList)
                        {
                            await Echo(bingNews.Title);
                        }
                        List<BingNews> bingNewsList1 = await task.GetNews("科技");

                        Console.WriteLine(bingNewsList.Count);
                        foreach (BingNews bingNews in bingNewsList1)
                        {
                            await Echo(bingNews.Title);
                        }
                        await Echo("Done");
                        break;

                    case "c9article":
                        var task1 = new Channel9Task();
                        //获取最近5页articles
                        for (int i = 5; i >= 1; i--)
                        {
                            List<C9Articles> articles = await task1.SaveArticles(i);
                            if (articles == null) continue;
                            foreach (C9Articles c9Article in articles)
                            {
                                await Echo("article:" + c9Article?.Title);
                            }
                        }
                        // 更新视频页内容
                        var videos = await task1.SaveVideosAsync(0, 60);
                        if (videos != null)
                        {
                            foreach (C9Videos video in videos)
                            {
                                await Echo("video:" + video?.Title);
                            }
                        }
                        await Echo("Done");
                        break;

                    case "mvavideos":
                        var task2 = new MvaTask();
                        List<MvaVideos> re = await task2.SaveMvaVideo();
                        foreach (MvaVideos video in re)
                        {
                            var newDetails = task2.GetMvaDetailAsync(video).Result;
                            await Echo("video:" + video?.Title);
                            if (newDetails?.Count > 0)
                            {
                                await Echo("\t包括子视频：" + newDetails?.Count + "个");
                            }
                            else
                            {
                                await Echo("\t没有子视频或获取子视频失败");
                            }
                        }
                        await Echo("Done");
                        break;
                    case "mvadetails":
                        var task3 = new MvaTask();
                        List<MvaVideos> videoList = await task3.UpdateRecentDetailAsync();
                        foreach (MvaVideos video in videoList)
                        {
                            await Echo("video:" + video?.Title);
                        }
                        await Echo("Done");
                        break;

                    case "devblogs":
                        var task4 = new DevBlogsTask();
                        var rssnews = task4.GetNewsAsync().Result;
                        foreach (var news in rssnews)
                        {
                            await Echo("devblog:" + news?.Title);
                        }
                        await Echo("Done");
                        break;
                    default:
                        break;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private async Task Echo(string message)
        {
            if (!IsNullOrEmpty(message))
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(
                  new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }

        }
    }
}
