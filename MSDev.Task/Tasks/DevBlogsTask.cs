using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSDev.Task.Entities;
using MSDev.Task.EnumTypes;
using MSDev.Task.Helpers;
using MSDev.Task.Models;
using Newtonsoft.Json;

namespace MSDev.Task.Tasks
{
    public class DevBlogsTask
    {
        private readonly ApiHelper _apiHelper;
        private const string devBlogsFeedsLink = "http://sxp.microsoft.com/feeds/3.0/devblogs";

        public DevBlogsTask(ApiHelper apiHelper)
        {
          _apiHelper = apiHelper;
        }

		public async Task<bool> GetNewsAsync()
		{
			ICollection<RssEntity> blogs = await RssHelper.GetRss(devBlogsFeedsLink);
			//var lastNews = await repository.DbSet.Where(x => x.Type == NewsTypes.DevBlog).LastOrDefaultAsync();
			IEnumerable<RssNews> _RssNews = blogs.OrderBy(x => x.PublishId).Select(x => new RssNews
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
				Type = NewsTypes.DevBlog,
				Status = ItemStatus.正常
			});

			Console.WriteLine(JsonConvert.SerializeObject(_RssNews));

			return true;
			//return await repository.AddRangeAsync(_RssNews) == _RssNews.Count();
		}
	}
}