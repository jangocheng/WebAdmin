using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MsDev.Taskschd.Entities;
using System;

namespace MsDev.Taskschd.Helpers
{
    public class BingSearchCrawler
    {
        #region Define Attributes

        private static string ImageSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/images/search";
        private static string AutoSuggestionEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/suggestions";
        private static string NewsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/news/search";

        private static HttpClient autoSuggestionClient { get; set; }
        private static HttpClient searchClient { get; set; }

        private static string autoSuggestionApiKey;

        public static string AutoSuggestionApiKey
        {
            get { return autoSuggestionApiKey; }

            set
            {
                var changed = autoSuggestionApiKey != value;

                autoSuggestionApiKey = value;

                if (changed)
                {
                    InitializeBingClients();
                }
            }
        }

        private static string searchApiKey;

        public static string SearchApiKey
        {
            get { return searchApiKey; }

            set
            {
                var changed = searchApiKey != value;

                searchApiKey = value;

                if (changed)

                {
                    InitializeBingClients();
                }
            }
        }

        #endregion Define Attributes

        private static void InitializeBingClients()
        {
            autoSuggestionClient = new HttpClient();
            autoSuggestionClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AutoSuggestionApiKey);
            searchClient = new HttpClient();
            searchClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SearchApiKey);
        }

        public static async Task<IEnumerable<string>> GetImageSearchResults(string query, string imageContent = "Face", int count = 20, int offset = 0)
        {
            var urls = new List<string>();

            var result = await searchClient.GetAsync(string.Format("{0}?q={1}&safeSearch=Strict&imageType=Photo&color=ColorOnly&count={2}&offset={3}{4}", ImageSearchEndPoint, WebUtility.UrlEncode(query), count, offset, string.IsNullOrEmpty(imageContent) ? "" : "&imageContent=" + imageContent));

            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);
            if (data.value != null && data.value.Count > 0)
            {
                for (var i = 0; i < data.value.Count; i++)
                {
                    urls.Add(data.value[i].contentUrl.Value);
                }
            }
            return urls;
        }

        public static async Task<IEnumerable<string>> GetAutoSuggestResults(string query, string market = "en-US")
        {
            var suggestions = new List<string>();
            var result = await autoSuggestionClient.GetAsync(string.Format("{0}/?q={1}&mkt={2}", AutoSuggestionEndPoint, WebUtility.UrlEncode(query), market));

            result.EnsureSuccessStatusCode();

            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);
            if (data.suggestionGroups != null && data.suggestionGroups.Count > 0 &&
                    data.suggestionGroups[0].searchSuggestions != null)
            {
                for (var i = 0; i < data.suggestionGroups[0].searchSuggestions.Count; i++)
                {
                    suggestions.Add(data.suggestionGroups[0].searchSuggestions[i].displayText.Value);
                }
            }
            return suggestions;
        }

        /// <summary>
        /// 获取必应新闻列表
        /// </summary>
        /// <param name="query">搜索关键词</param>
        /// <param name="count">数量</param>
        /// <param name="offset">偏移量</param>
        /// <param name="market">地区</param>
        /// <param name="freshness">时间频率</param>
        /// <returns></returns>
        public static async Task<List<BingNewsEntity>> GetNewsSearchResults(string query, int count = 20, int offset = 0, string market = "zh-CN", string freshness = "Day")
        {
            var articles = new List<BingNewsEntity>();
            var result = await searchClient.GetAsync(string.Format("{0}/?q={1}&count={2}&offset={3}&mkt={4}&freshness={5}", NewsSearchEndPoint, WebUtility.UrlEncode(query), count, offset, market, freshness));

            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);

            if (data.value != null && data.value.Count > 0)
            {
                for (var i = 0; i < data.value.Count; i++)
                {
                    var news = new BingNewsEntity
                    {
                        Title = data.value[i].name,
                        Url = data.value[i].url,
                        Description = data.value[i].description,
                        ThumbnailUrl = data.value[i].image?.thumbnail?.contentUrl,
                        Provider = data.value[i].provider?[0].name,
                        DatePublished = data.value[i].datePublished,
                        CateGory = data.value[i].category
                    };
                    if (!string.IsNullOrEmpty(news.ThumbnailUrl))
                    {
                        articles.Add(news);
                    }
                }
            }
            return articles;
        }
    }
}