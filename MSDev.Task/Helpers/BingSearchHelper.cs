using MSDev.Work.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSDev.Work.Helpers
{
    public class BingSearchHelper
    {
        #region Define Attributes

        private const string ImageSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/images/search";
        private const string AutoSuggestionEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/suggestions";
        private const string NewsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/news/search";
        private const string TopNewsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/news";


        private static HttpClient AutoSuggestionClient { get; set; }
        private static HttpClient SearchClient { get; set; }

        private static string _autoSuggestionApiKey;

        public static string AutoSuggestionApiKey
        {
            get
            {
                return _autoSuggestionApiKey;
            }

            set
            {
                bool changed = _autoSuggestionApiKey != value;

                _autoSuggestionApiKey = value;

                if (changed)
                {
                    InitializeBingClients();
                }
            }
        }

        private static string _searchApiKey;

        public static string SearchApiKey
        {
            get
            {
                return _searchApiKey;
            }

            set
            {
                bool changed = _searchApiKey != value;

                _searchApiKey = value;

                if (changed)

                {
                    InitializeBingClients();
                }
            }
        }

        #endregion Define Attributes

        private static void InitializeBingClients()
        {
            AutoSuggestionClient = new HttpClient();
            AutoSuggestionClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AutoSuggestionApiKey);
            SearchClient = new HttpClient();
            SearchClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SearchApiKey);
        }

        public static async Task<IEnumerable<string>> GetImageSearchResults(string query, string imageContent = "Face", int count = 20, int offset = 0)
        {
            var urls = new List<string>();

            HttpResponseMessage result = await SearchClient.GetAsync(
              $"{ImageSearchEndPoint}?q={WebUtility.UrlEncode(query)}&safeSearch=Strict&imageType=Photo&color=ColorOnly&count={count}&offset={offset}{(string.IsNullOrEmpty(imageContent) ? "" : "&imageContent=" + imageContent)}");

            result.EnsureSuccessStatusCode();
            string json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);
            if (data.value == null || data.value.Count <= 0)
            {
                return urls;
            }

            for (int i = 0; i < data.value.Count; i++)
            {
                urls.Add(data.value[i].contentUrl.Value);
            }
            return urls;
        }

        public static async Task<IEnumerable<string>> GetAutoSuggestResults(string query, string market = "en-US")
        {
            var suggestions = new List<string>();
            HttpResponseMessage result = await AutoSuggestionClient.GetAsync(string.Format("{0}/?q={1}&mkt={2}", AutoSuggestionEndPoint, WebUtility.UrlEncode(query), market));

            result.EnsureSuccessStatusCode();

            string json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);
            if (data.suggestionGroups == null || data.suggestionGroups.Count <= 0 ||
                data.suggestionGroups[0].searchSuggestions == null)
            {
                return suggestions;
            }
            for (int i = 0; i < data.suggestionGroups[0].searchSuggestions.Count; i++)
            {
                suggestions.Add(data.suggestionGroups[0].searchSuggestions[i].displayText.Value);
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
            try
            {
                HttpResponseMessage result = await SearchClient.GetAsync(
              $"{NewsSearchEndPoint}/?q={WebUtility.UrlEncode(query)}&count={count}&offset={offset}&mkt={market}&freshness={freshness}");

                result.EnsureSuccessStatusCode();
                string json = await result.Content.ReadAsStringAsync();
                dynamic data = JObject.Parse(json);

                if (data.value == null || data.value.Count <= 0)
                {
                    return articles;
                }

                for (int i = 0; i < data.value.Count; i++)
                {
                    var news = new BingNewsEntity
                    {
                        Title = data.value[i].name,
                        Url = data.value[i].url,
                        Description = data.value[i].description,
                        ThumbnailUrl = data.value[i].image?.thumbnail?.contentUrl,
                        Provider = data.value[i].provider?[0].name,
                        DatePublished = data.value[i].datePublished,
                        Category = data.value[i].category
                    };
                    if (!string.IsNullOrEmpty(news.ThumbnailUrl))
                    {
                        articles.Add(news);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source + e.Message);
            }

            return articles;
        }


        public static async Task<List<BingNewsEntity>> GetTopNews(string category = "ScienceAndTechnology")
        {
            var articles = new List<BingNewsEntity>();
            try
            {
                HttpResponseMessage result = await SearchClient.GetAsync(
              $"{TopNewsSearchEndPoint}/?category={category}");

                result.EnsureSuccessStatusCode();
                string json = await result.Content.ReadAsStringAsync();
                dynamic data = JObject.Parse(json);

                if (data.value == null || data.value.Count <= 0)
                {
                    return articles;
                }

                for (int i = 0; i < data.value.Count; i++)
                {
                    var news = new BingNewsEntity
                    {
                        Title = data.value[i].name,
                        Url = data.value[i].url,
                        Description = data.value[i].description,
                        ThumbnailUrl = data.value[i].image?.thumbnail?.contentUrl,
                        Provider = data.value[i].provider?[0].name,
                        DatePublished = data.value[i].datePublished,
                        Category = data.value[i].category
                    };
                    if (!string.IsNullOrEmpty(news.ThumbnailUrl))
                    {
                        articles.Add(news);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source + e.Message);
            }
            return articles;
        }
    }
}