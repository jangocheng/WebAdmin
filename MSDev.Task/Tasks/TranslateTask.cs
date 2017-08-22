using MSDev.DB.Entities;
using MSDev.Work.Helpers;

namespace MSDev.Work.Tasks
{
    public class TranslateTask : MSDTask
    {
        private readonly string Key = "";

        static TranslateTextHelper _helper;

        public TranslateTask()
        {
            Key = Configuration.GetSection("TranslateKey")?.Value;
            _helper = new TranslateTextHelper(Key);
        }

        public bool TranslateBlog(RssNews rss)
        {
            var content = _helper.TranslateText(rss.Description);
            var title = _helper.TranslateText(rss.Title);
            var oldNews = Context.RssNews.Find(rss.Id);
            oldNews.Content = content;
            oldNews.TitleCn = title;
            Context.Update(oldNews);
            var re = Context.SaveChanges();
            return re > 0;
        }

    }
}