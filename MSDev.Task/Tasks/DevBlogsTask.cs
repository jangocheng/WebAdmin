using MSDev.DB.Entities;
using MSDev.Work.Entities;
using MSDev.Work.Helpers;
using MSDev.Work.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSDev.Work.Tasks
{
    public class DevBlogsTask : MSDTask
    {
        private const string devBlogsFeedsLink = "https://blogs.technet.microsoft.com/cloudplatform/rssfeeds/devblogs";

        public async Task<List<RssNews>> GetNewsAsync()
        {
            List<RssEntity> blogs = await RssHelper.GetRss(devBlogsFeedsLink);

            //var lastNews = await repository.DbSet.Where(x => x.Type == NewsTypes.DevBlog).LastOrDefaultAsync();
            List<RssNews> rssnews = blogs.OrderBy(x => x.LastUpdateTime).Select(x => new RssNews
            {
                Title = x.Title,
                Author = x.Author,
                Description = x.Description,
                Categories = x.Categories,
                CreateTime = x.CreateTime,
                LastUpdateTime = x.LastUpdateTime,
                Link = x.Link,
                PublishId = x.PublishId,
                MobileContent = x.MobileContent,
                Type = 1,
                Status = 1
            }).ToList();
            //先去重源数据
            for (int i = 0; i < rssnews.Count - 1; i++)
            {
                for (int j = i + 1; j < rssnews.Count; j++)
                {
                    if (rssnews[i].Title.Equals(rssnews[j].Title))
                    {
                        rssnews[i].Title = string.Empty;
                        break;
                    }
                }
            }

            //取最新数据，去重 
            var oldData = Context.RssNews.OrderByDescending(m => m.LastUpdateTime).Take(20).ToList();
            var toBeAdd = rssnews.FindAll(NotSame);


            //判断重复
            bool NotSame(RssNews news)
            {

                if (string.IsNullOrEmpty(news.Title)) return false;
                if (oldData.Any(m => m.Title.Equals(news.Title)))
                {
                    Console.WriteLine("重复" + news.Title);
                    return false;
                }
                return true;
            }

            // 添加翻译内容
            if (toBeAdd.Count < 1) return toBeAdd;
            var key = Configuration.GetSection("TranslateKey")?.Value;
            var translateHelper = new TranslateTextHelper(key);
            try
            {
                foreach (var item in toBeAdd)
                {
                    item.Content = translateHelper.TranslateText(item.Description);
                    item.TitleCn = translateHelper.TranslateText(item.Title);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Log.Write("errors.txt", "翻译:" + e.Source + e.InnerException.Message, true);
            }
            //插入新数据
            Context.RssNews.AddRange(toBeAdd);
            var re = Context.SaveChanges();
            return toBeAdd;
        }
    }
}