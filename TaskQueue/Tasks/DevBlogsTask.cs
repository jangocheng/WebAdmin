using MsDev.Taskschd.Helpers;
using MSDev.DataAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MsDev.Taskschd.Tasks
{
    public class DevBlogsTask
    {
        private AppDbContext db = null;
        private const string devBlogsFeedsLink = "http://sxp.microsoft.com/feeds/3.0/devblogs";

        public DevBlogsTask(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> GetNews()
        {
            var blogs = RssCrawler.GetRss(devBlogsFeedsLink);
            return await Task.FromResult(false);
        }
    }
}