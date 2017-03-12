using MsDev.Taskschd.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System;

namespace MsDev.Taskschd.Helpers
{
    public static class RssCrawler
    {
        private readonly static HttpClient httpClient;

        private const string devBlogsFeedsLink = "http://sxp.microsoft.com/feeds/3.0/devblogs";
        private const string cloudFeedsLink = "https://sxp.microsoft.com/feeds/3.0/cloud";

        static RssCrawler()
        {
            if (httpClient == null)
                httpClient = new HttpClient();
        }

        public static async Task<ICollection<RssEntity>> GetRss(string url)
        {
            var blogs = new List<RssEntity>();
            var xml = await httpClient.GetStringAsync(url);
            if (!string.IsNullOrEmpty(xml))
            {
                using (var reader = XmlReader.Create(new StringReader(xml.Replace("sxp:", "sxp_"))))
                {
                    reader.MoveToElement();
                    XDocument xRoot = XDocument.Load(reader, LoadOptions.SetLineInfo);

                    blogs = xRoot.Document.Element("rss").Element("channel").Elements().Where(x => x.Name == "item")
                        .Select(x => new RssEntity
                        {
                            Title = x.Element("title").Value,
                            Categories = x.Elements().Where(n => n.Name == "category").Select(n => n.Value).ToList(),
                            Description = x.Element("description").Value,
                            CreateTime = DateTime.Parse(x.Element("pubDate").Value),
                            Author = x.Element("sxp_Author").Value,
                            LastUpdateTime = DateTime.Parse(x.Element("sxp_LastUpdated").Value),
                            Link = x.Element("link").Value,
                            MobileContent = x.Element("sxp_MobileContent").Value,
                            PublishId = int.Parse(x.Element("sxp_PublishId").Value)
                        })
                        .ToList();
                }
            }
            return blogs;
        }

        public static async Task<ICollection<RssEntity>> GetDevBlogs()
        {
            return await GetRss(devBlogsFeedsLink);
        }

        public static async Task<ICollection<RssEntity>> GetCloudNews()
        {
            return await GetRss(cloudFeedsLink);
        }
    }
}