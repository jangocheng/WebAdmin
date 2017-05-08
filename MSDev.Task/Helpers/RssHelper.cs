using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System;
using MSDev.Task.Entities;

namespace MSDev.Task.Helpers
{
    public static class RssHelper
    {
        private readonly static HttpClient httpClient;

        private const String devBlogsFeedsLink = "http://sxp.microsoft.com/feeds/3.0/devblogs";
        private const String cloudFeedsLink = "https://sxp.microsoft.com/feeds/3.0/cloud";

        static RssHelper()
        {
            if (httpClient == null)
                httpClient = new HttpClient();
        }

        public static async Task<ICollection<RssEntity>> GetRss(String url)
        {
            List<RssEntity> blogs = new List<RssEntity>();
            String xml = await httpClient.GetStringAsync(url);
            if (!String.IsNullOrEmpty(xml))
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xml.Replace("sxp:", "sxp_"))))
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
                            PublishId = Int32.Parse(x.Element("sxp_PublishId").Value)
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