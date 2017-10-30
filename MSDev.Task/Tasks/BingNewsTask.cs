using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MSDev.DB.Entities;
using MSDev.Work.Entities;
using MSDev.Work.Helpers;
using MSDev.Work.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.String;
namespace MSDev.Work.Tasks
{
    public class BingNewsTask : MSDTask
    {
        private readonly string BingSearchKey = "";
        private const string Domain = "http://msdev.cc/";//TODO: [域名]读取配置
        private const double Similarity = 0.5;//定义相似度

        public BingNewsTask()
        {
            BingSearchKey = Configuration.GetSection("BingSearchKey")?.Value;

        }

        public async Task<List<BingNews>> GetNews(string keyword, string freshness = "Day")
        {
            //获取新闻
            if (IsNullOrEmpty(BingSearchKey))
            {
                return default;
            }
            BingSearchHelper.SearchApiKey = BingSearchKey;
            List<BingNewsEntity> newNews = await BingSearchHelper.GetNewsSearchResults(keyword);
            if (newNews == null)
            {
                return default;
            }

            //TODO:获取过滤来源名单
            string[] providerFilter = { "大连天健网", "中金在线", "安卓网资讯专区", "中国通信网", "中国网", "华商网", "A5站长网", "东方财富网 股票", "秦巴在线", "ITBEAR科技资讯", "京华网", "TechWeb", "四海网" };

            //数据预处理
            for (int i = 0; i < newNews.Count; i++)
            {
                //来源过滤
                if (Array.Exists(providerFilter, provider => provider == newNews[i].Provider))
                {
                    Console.WriteLine("filter:" + newNews[i].Provider + newNews[i].Title);
                    newNews[i].Title = Empty;
                    continue;
                }
                //无缩略图过滤
                if (IsNullOrEmpty(newNews[i].ThumbnailUrl))
                {
                    Console.WriteLine("noPic:" + newNews[i].Title);
                    newNews[i].Title = Empty;
                    continue;
                }

                //TODO: 语义分词重复过滤
                for (int j = i + 1; j < newNews.Count; j++)
                {
                    //重复过滤
                    if (!(StringTools.Similarity(newNews[i].Title, newNews[j].Title) > Similarity))
                    {
                        continue;
                    }
                    Console.WriteLine("repeat" + newNews[i].Title);
                    newNews[i].Title = Empty;
                }

            }
            //查询库中内容并去重
            var oldTitles = Context.BingNews
                .OrderByDescending(m => m.UpdatedTime)
                .Where(m => m.Tags.Equals(keyword))
                .Select(m => m.Title)
                .Take(50)
                .ToList();

            foreach (BingNewsEntity t in newNews)
            {
                if (IsNullOrEmpty(t.Title))
                {
                    continue;
                }

                foreach (string oldTitle in oldTitles)
                {
                    if (!(StringTools.Similarity(t.Title, oldTitle) > Similarity))
                    {
                        continue;
                    }

                    Console.WriteLine("repeat:" + t.Title);
                    t.Title = Empty;
                    break;
                }
            }

            //去重后的内容
            var newsTba = new List<BingNews>();
            foreach (BingNewsEntity item in newNews)
            {
                if (IsNullOrEmpty(item.Title))
                {
                    continue;
                }
                //对比库中标题，最后去重
                if (Context.BingNews.Any(m => m.Title.Equals(item.Title)))
                {
                    continue;
                }
                Console.WriteLine("New News:" + item.Title);

                var targetUrl = Domain + "?r=" + item.Url;
                var news = new BingNews
                {
                    Id = Guid.NewGuid(),
                    Title = item.Title,
                    Description = item.Description,
                    Url = targetUrl,
                    ThumbnailUrl = item.ThumbnailUrl,
                    Status = 0,
                    Tags = keyword,
                    Provider = item.Provider,
                    CreatedTime = item.DatePublished,
                    UpdatedTime = DateTime.Now
                };
                newsTba.Add(news);
            }
            Context.AddRange(newsTba);
            int re = Context.SaveChanges();
            Console.WriteLine($"Update {re} news!");
            return newsTba;
        }

    }
}