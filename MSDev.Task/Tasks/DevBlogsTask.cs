using System;
using System.Collections.Generic;
using MSDev.DataAgent.Models;
using System.Linq;
using System.Threading.Tasks;
using MSDev.Task.Entities;
using MSDev.Task.Helpers;
using Newtonsoft.Json;

namespace MSDev.Task.Tasks
{
    public class DevBlogsTask
    {
        private readonly ApiHelper _apiHelper;
        private const String devBlogsFeedsLink = "http://sxp.microsoft.com/feeds/3.0/devblogs";

        public DevBlogsTask(ApiHelper apiHelper)
        {
          _apiHelper = apiHelper;
        }

        public async Task<Boolean> GetNews()
        {
            ICollection<RssEntity> blogs = await RssHelper.GetRss(devBlogsFeedsLink);
            var lastNews = await repository.DbSet.Where(x => x.Type == NewsTypes.DevBlog).LastOrDefaultAsync();
            var _RssNews = blogs.Where(x => x.PublishId > lastNews.PublishId).OrderBy(x => x.PublishId).Select(x => new RssNews
            {
                Title = x.Title,
                Author = x.Author,
                Description = x.Description,
                Categories = JsonConvert.SerializeObject(x.Categories),
                CreateTime = x.CreateTime,
                LastUpdateTime = x.LastUpdateTime,
                Link = x.Link,
                PublishId = x.PublishId,
                MobileContent = x.MobileContent,
                Type = NewsTypes.DevBlog,
                Status = ItemStatus.正常
            });
            return await repository.AddRangeAsync(_RssNews) == _RssNews.Count();
        }
    }
}