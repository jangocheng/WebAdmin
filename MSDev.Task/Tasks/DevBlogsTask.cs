using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSDev.DB.Entities;
using MSDev.Task.Entities;
using MSDev.Task.EnumTypes;
using MSDev.Task.Helpers;

namespace MSDev.Task.Tasks
{
    public class DevBlogsTask : MSDTask
    {
        private const string devBlogsFeedsLink = "https://blogs.technet.microsoft.com/cloudplatform/rssfeeds/devblogs";

        public async Task<List<RssNews>> GetNewsAsync()
        {
            ICollection<RssEntity> blogs = await RssHelper.GetRss(devBlogsFeedsLink);

            //var lastNews = await repository.DbSet.Where(x => x.Type == NewsTypes.DevBlog).LastOrDefaultAsync();
            IEnumerable<RssNews> rssnews = blogs.OrderBy(x => x.PublishId).Select(x => new RssNews
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
            });
            var toBeAdd = new List<RssNews>(rssnews);//待添加数据

            //取最新数据，去重 
            var oldData = Context.RssNews.OrderByDescending(m => m.LastUpdateTime).Take(20).ToList();
            foreach (var news in rssnews)
            {
                foreach (var data in oldData)
                {
                    if (news.Title.Equals(data.Title))
                    {
                        toBeAdd.Remove(news);
                        break;
                    }
                }
            }
            //插入新数据
            Context.RssNews.AddRange(toBeAdd);
            var re = Context.SaveChanges();
            return toBeAdd;
        }
    }
}