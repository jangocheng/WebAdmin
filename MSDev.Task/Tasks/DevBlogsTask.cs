using MSDev.Work.Entities;
using MSDev.Work.Helpers;
using MSDev.Work.Tools;

namespace MSDev.Work.Tasks
{
    public class DevBlogsTask : MSDTask
    {
        private const string devBlogsFeedsLink = "https://blogs.technet.microsoft.com/cloudplatform/rssfeeds/devblogs";

        public async Task<List<RssNews>> GetNewsAsync()
        {
            ICollection<RssEntity> blogs = await RssHelper.GetRss(devBlogsFeedsLink);

            //var lastNews = await repository.DbSet.Where(x => x.Type == NewsTypes.DevBlog).LastOrDefaultAsync();
            IEnumerable<RssNews> rssnews = blogs.OrderBy(x => x.LastUpdateTime).Select(x => new RssNews
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

            var toBeAdd = new List<RssNews>();//待添加数据
            //取最新数据，去重 
            var oldData = Context.RssNews.OrderByDescending(m => m.LastUpdateTime).Take(20).ToList();
            foreach (var news in rssnews)
            {
                var isNew = true;
                foreach (var data in oldData)
                {
                    if (news.Title.Equals(data.Title))
                    {
                        Console.WriteLine("重复" + news.Title);
                        isNew = false;
                        break;
                    }
                }
                if (isNew)
                    toBeAdd.Add(news);
            }

            // 添加翻译内容
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