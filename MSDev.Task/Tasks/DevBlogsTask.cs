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
				Type = NewsTypes.DevBlog,
				Status = ItemStatus.正常
			});

			var re = await _apiHelper.Post<int>("/api/manage/rssnews", rssnews);


			if(re.ErrorCode==0){
				return true;
			}
			// 插入错误
			Console.WriteLine(re.Msg);
			return false;

			//return await repository.AddRangeAsync(_RssNews) == _RssNews.Count();
		}
	}
}