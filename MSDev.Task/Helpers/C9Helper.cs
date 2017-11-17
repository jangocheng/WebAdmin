using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Internal;
using MSDev.DB.Entities;
using MSDev.Work.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.String;

namespace MSDev.Work.Helpers
{
    public class C9Helper
    {
        private const string BeginUrl = "https://channel9.msdn.com/Browse/AllContent?lang=en&lang=zh-cn&lang=zh-tw";
        private const string C9Daemon = "https://channel9.msdn.com/";
        private static string[] Events = { "Build", "Ignite", "connect", "dotnetConf" };

        private static readonly HttpClient HttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) };
        public C9Helper()
        {

        }

        /// <summary>
        /// 获取 列表分页总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetTotalPage(string url = "")
        {
            url = IsNullOrWhiteSpace(url) ? BeginUrl : url;
            int pageNumber = 0;
            using (var hc = new HttpClient())
            {
                string htmlString = await hc.GetStringAsync(url);
                if (string.IsNullOrEmpty(htmlString)) return pageNumber;
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlString);

                string totalPage = htmlDoc.DocumentNode
                    .SelectSingleNode("//main/div[contains(@class,'paging nav holder')]//li/span[@class='ellip']")
                    .ParentNode.SelectSingleNode(".//a")
                    .InnerText; //总页数
                pageNumber = int.Parse(totalPage);
                return pageNumber;
            }
        }

        /// <summary>
        /// 抓取事件视频
        /// </summary>
        /// <param name="sourceUrl"></param>
        public async Task<List<EventVideo>> GetEventVideosAsync(C9Event c9Event)
        {
            var c9EventVideos = new List<EventVideo>();
            var pageNumber = await GetTotalPage(c9Event.SourceUrl);
            for (int i = 1; i <= pageNumber; i++)
            {
                //获取列表内容
                var list = await GetArticleListAsync(i, c9Event.SourceUrl);
                //获取详情内容
                foreach (var item in list)
                {
                    var video = await GetPageVideo(item);
                    var eventVideo = new EventVideo
                    {
                        Id = Guid.NewGuid(),
                        Author = video.Author,
                        Caption = video.Caption,
                        CreatedTime = video.CreatedTime,
                        Description = video.Description,
                        Duration = video.Duration,
                        Language = video.Language,
                        Mp3Url = video.Mp3Url,
                        Mp4HigUrl = video.Mp4HigUrl,
                        Mp4LowUrl = video.Mp4LowUrl,
                        Mp4MidUrl = video.Mp4MidUrl,
                        SeriesTitle = video.SeriesTitle,
                        SeriesTitleUrl = video.SeriesTitleUrl,
                        SeriesType = video.SeriesType,
                        SourceUrl = video.SourceUrl,
                        Status = video.Status,
                        Tags = video.Tags,
                        ThumbnailUrl = video.ThumbnailUrl,
                        Title = video.Title,
                        UpdatedTime = video.UpdatedTime,
                        VideoEmbed = video.VideoEmbed,
                        Views = video.Views,
                        C9Event = c9Event
                    };
                    c9EventVideos.Add(eventVideo);
                }
            }
            return c9EventVideos;
        }

        /// <summary>
        /// 获取视频列表
        /// </summary>
        public async Task<List<C9Articles>> GetArticleListAsync(int page = 1, string url = "")
        {
            var articleList = new List<C9Articles>();
            try
            {
                url = IsNullOrWhiteSpace(url) ? BeginUrl : url;
                url = url + "&page=" + page;
                string htmlString = await HttpClient.GetStringAsync(url);
                if (!IsNullOrEmpty(htmlString))
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlString);
                    // 解析获取内容列表
                    articleList = htmlDoc.DocumentNode.Descendants("article")
                        .Where(n => n.Attributes.Contains("data-api"))
                        .Select(n => new C9Articles
                        {
                            Id = Guid.NewGuid(),
                            SeriesTitle = n.SelectSingleNode(".//div[@class='seriesTitle']/a")?.InnerText,
                            SeriesTitleUrl = n.SelectSingleNode(".//div[@class='seriesTitle']/a")?.GetAttributeValue("href", Empty),
                            Title = n.SelectSingleNode(".//header/h3/a")?.InnerText,
                            SourceUrl = n.SelectSingleNode(".//header/h3/a")?.GetAttributeValue("href", Empty),
                            ThumbnailUrl = n.SelectSingleNode(".//a/img")?.GetAttributeValue("src", Empty),
                            Duration = n.SelectSingleNode(".//a/time")?.GetAttributeValue("datetime", Empty),
                            Status = 0,
                            CreatedTime = DateTime.Now.AddDays(-page),
                            UpdatedTime = DateTime.Now.AddDays(-page)
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ex.Message);
            }
            return articleList;
        }

        // 临时补充遗漏
        public C9Videos GetPageVideoByUrl(string fullUrl)
        {
            C9Videos video = new C9Videos();
            try
            {
                var hw = new HtmlWeb();
                // option获取InnerText，需要加以下设置
                HtmlNode.ElementsFlags.Remove("option");
                HtmlDocument htmlDoc = hw.Load(fullUrl);
                HtmlNode mainNode = htmlDoc.DocumentNode.SelectSingleNode(".//main[@role='main']");
                video.Duration = mainNode.SelectSingleNode(".//div[@class='playerContainer']//time[@class='caption']")?
                    .Attributes["datetime"]?.Value;
                // 非视频,返回
                if (video.Duration == null)
                {
                    return video;
                }
                video.SeriesTitle = mainNode.SelectSingleNode(".//div[@class='seriesTitle']//a")?.InnerText;
                video.SeriesTitleUrl = mainNode.SelectSingleNode(".//div[@class='seriesTitle']//a")?.Attributes["href"]?.Value;
                video.SourceUrl = fullUrl;
                video.Title = mainNode.SelectSingleNode(".//div[@class='itemHead holder']//div[@class='container']//h1")?.InnerText;
                video.ThumbnailUrl = mainNode.SelectSingleNode(".//div[@class='playerContainer']//a[@class='video']")
                    ?.Attributes["style"]?.Value;
                if (!IsNullOrEmpty(video.ThumbnailUrl))
                {
                    Regex regex = new Regex(@".+\((.+)\);");
                    video.ThumbnailUrl = regex.Match(video.ThumbnailUrl).Groups[1].Value;
                }
                video.Author = mainNode.SelectSingleNode(".//div[@class='authors']")?.Descendants("a")?.Select(s => s.InnerText)
                    .ToArray().Join();

                video.Language = mainNode.SelectSingleNode(".//div[@class='itemHead holder' and @dir='ltr']")?
                    .GetAttributeValue("lang", Empty);
                video.Description = mainNode.SelectSingleNode(".//section[@class='ch9tab description']/div[@class='ch9tabContent']")
                    .InnerHtml;
                var downloadUrls = mainNode.SelectNodes(".//section[@class='ch9tab download']//div[@class='download']//ul//li")?
                .Select(s => new
                {
                    text = s.Element("a").Attributes["download"]?.Value,
                    value = s.Element("a").Attributes["href"]?.Value
                }).ToList();

                Console.WriteLine(JsonConvert.SerializeObject(downloadUrls));
                foreach (var downloadUrl in downloadUrls)
                {
                    var downloadType = downloadUrl.text.ToLower().Trim();
                    if (downloadType.Contains(".mp3"))
                    {
                        video.Mp3Url = downloadUrl.value;
                    }
                    else if (downloadType.Contains("low.mp4"))
                    {
                        video.Mp4LowUrl = downloadUrl.value;
                    }
                    else if (downloadType.Contains("mid.mp4"))
                    {
                        video.Mp4MidUrl = downloadUrl.value;
                    }
                    else if (downloadType.Contains("high.mp4"))
                    {
                        video.Mp4HigUrl = downloadUrl.value;
                    }
                }


                video.Tags = mainNode
                    .SelectNodes(".//section[@class='ch9tab description']//div[@class='ch9tabContent']//div[@class='tags']//a")?
                    .Select(s => s.InnerText).ToArray()?.Join();

                video.Views = 0;
                video.CreatedTime = DateTime.Parse(mainNode.SelectSingleNode(".//time[@class='timeHelper']")?
                    .GetAttributeValue("datetime", Empty));

                video.UpdatedTime = video.CreatedTime;

            }
            catch (Exception e)
            {

                Log.Write("c9videoError.txt", fullUrl);
                Console.WriteLine($"The Error:{e}");
            }
            return video;

        }

        /// <summary>
        /// 抓取单页视频内容
        /// </summary>
        /// <param name="article"></param>
        /// <param name="fullUrl"></param>
        /// <returns></returns>
        public async Task<C9Videos> GetPageVideo(C9Articles article)
        {
            var video = new C9Videos
            {
                Duration = article.Duration,
                SeriesTitle = article.SeriesTitle,
                SeriesTitleUrl = article.SeriesTitleUrl,
                SourceUrl = article.SourceUrl,
                Title = article.Title,
                ThumbnailUrl = article.ThumbnailUrl
            };

            video.SeriesType = article.SeriesTitleUrl.Substring(1);
            video.SeriesType = video.SeriesType.Substring(0, video.SeriesType.IndexOf(@"/"));

            string url = C9Daemon + article.SourceUrl;
            try
            {
                var hw = new HtmlWeb();
                // option获取InnerText，需要加以下设置
                HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("option");
                HtmlDocument htmlDoc = await hw.LoadFromWebAsync(url);

                HtmlNode mainNode = htmlDoc.DocumentNode.SelectSingleNode(".//main[@role='main']");

                video.Author = mainNode.SelectSingleNode(".//div[@class='authors']")?.Descendants("a")?.Select(s => s.InnerText)
                    .ToArray().Join();

                video.Language = mainNode.SelectSingleNode(".//div[@class='itemHead holder' and @dir='ltr']")?
                    .GetAttributeValue("lang", Empty);
                video.Description = mainNode.SelectSingleNode(".//section[@class='ch9tab description']/div[@class='ch9tabContent']")?
                    .InnerText;
                video.VideoEmbed = mainNode.SelectSingleNode(".//section[@class='ch9tab embed']/div[@class='ch9tabContent']")?
                    .InnerHtml;
                var downloadUrls = mainNode.SelectNodes(".//section[@class='ch9tab download']//div[@class='download']//ul//li")?
                     .Select(s => new
                     {
                         text = s.Element("a").Attributes["download"]?.Value,
                         value = s.Element("a").Attributes["href"]?.Value
                     }).ToList();

                foreach (var downloadUrl in downloadUrls)
                {

                    var downloadType = downloadUrl.text?.ToLower().Trim();
                    if (string.IsNullOrEmpty(downloadType)) continue;
                    if (downloadType.Contains(".mp3"))
                    {
                        video.Mp3Url = downloadUrl.value;
                    }
                    else if (downloadType.Contains("low.mp4"))
                    {
                        video.Mp4LowUrl = downloadUrl.value;
                    }
                    else if (downloadType.Contains("mid.mp4"))
                    {
                        video.Mp4MidUrl = downloadUrl.value;
                    }
                    else if (downloadType.Contains("high.mp4"))
                    {
                        video.Mp4HigUrl = downloadUrl.value;
                    }
                }
                video.Tags = mainNode
                    .SelectNodes(".//section[@class='ch9tab description']//div[@class='ch9tabContent']//div[@class='tags']//a")?
                    .Select(s => s.InnerText).ToArray()?.Join();
                video.Views = 0;
                video.CreatedTime = DateTime.Parse(mainNode.SelectSingleNode(".//time[@class='timeHelper']")?
                    .GetAttributeValue("datetime", Empty));
                if (video.CreatedTime == null) video.CreatedTime = DateTime.Now;
                video.Caption = mainNode.SelectSingleNode(".//section[@class='ch9tab download']//div[@class='download']/div[2]")?
                    .InnerHtml;
                video.UpdatedTime = video.CreatedTime;
                return video;
            }
            catch (Exception e)
            {
                Log.Write("c9videoGetErrors.txt", url + $";{e.Source}:{e.Message}");
                Console.WriteLine($"The Error:{url}");
            }
            return default;
        }

        /// <summary>
        /// 抓取事件
        /// </summary>
        /// <returns></returns>
        public async Task<List<C9Event>> GetEventsAsync()
        {
            var eventList = new List<C9Event>();

            using (var hc = new HttpClient())
            {
                foreach (var item in Events)
                {
                    var url = C9Daemon + "Events/" + item;
                    string htmlString = await hc.GetStringAsync(url);

                    if (IsNullOrEmpty(htmlString)) return default;
                    Console.WriteLine(htmlString);
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlString);

                    var C9Events = htmlDoc.DocumentNode
                        .SelectNodes("//main//section//article[@class='abstract xSmall noVideo']")
                        .Select(s => new C9Event
                        {
                            Id = Guid.NewGuid(),
                            CreatedTime = DateTime.Now,
                            EventDate = s.SelectSingleNode("//time")?.InnerText,
                            Language = s.Attributes["lang"]?.Value,
                            EventName = item,
                            SourceUrl = C9Daemon + s.SelectSingleNode("//a[@class='tile']")?.Attributes["href"]?.Value,
                            ThumbnailUrl = s.SelectSingleNode("//img")?.Attributes["src"]?.Value,
                            TopicName = s.SelectSingleNode("//header/h3/a").InnerHtml
                        })
                        .ToList();

                    eventList.AddRange(C9Events);
                }
            }
            return eventList;
        }
    }
}