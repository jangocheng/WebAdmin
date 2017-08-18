using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MSDev.DB.Entities;
using MSDev.Task.Entities;
using MSDev.Task.Helpers;
using MSDev.Task.Tools;
using static System.String;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MSDev.Task.Tasks
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
            var oldNews = Context.RssNews.Find(rss);
            oldNews.Content = content;
            oldNews.TitleCn = title;
            Context.Update(oldNews);
            var re = Context.SaveChanges();
            return re > 0;
        }

    }
}