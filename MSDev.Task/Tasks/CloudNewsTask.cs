using Microsoft.EntityFrameworkCore;
using MSDev.DataAgent.EnumTypes;
using MSDev.DataAgent.Models;
using MSDev.DataAgent.Repositories;
using MSDev.Taskschd.Helpers;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MSDev.Taskschd.Tasks
{
    public class CloudNewsTask
    {
        private IRssRepository repository = null;
        private const string CloudNewsFeedsLink = "http://sxp.microsoft.com/feeds/3.0/devblogs";

        public CloudNewsTask(IRssRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> GetNews()
        {
            var blogs = await RssCrawler.GetRss(CloudNewsFeedsLink);
            var lastNews = await repository.DbSet.Where(x => x.Type == NewsTypes.Cloud).LastOrDefaultAsync();
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
                Type = NewsTypes.Cloud,
                Status = ItemStatus.正常
            });
            return await repository.AddRangeAsync(_RssNews) == _RssNews.Count();
        }
    }
}