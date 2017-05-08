using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using MSDev.Task.Entities;

namespace MSDev.Task.Helpers
{
  public class BingSearchHelper
  {
    #region Define Attributes

    private const String ImageSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/images/search";
    private const String AutoSuggestionEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/suggestions";
    private const String NewsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/news/search";
    private static HttpClient AutoSuggestionClient { get; set; }
    private static HttpClient SearchClient { get; set; }

    private static String _autoSuggestionApiKey;

    public static String AutoSuggestionApiKey
    {
      get => _autoSuggestionApiKey;

      set {
        Boolean changed = _autoSuggestionApiKey != value;

        _autoSuggestionApiKey = value;

        if (changed)
        {
          InitializeBingClients();
        }
      }
    }

    private static String _searchApiKey;

    public static String SearchApiKey
    {
      get => _searchApiKey;

      set {
        Boolean changed = _searchApiKey != value;

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

    public static async Task<IEnumerable<String>> GetImageSearchResults(String query, String imageContent = "Face", Int32 count = 20, Int32 offset = 0)
    {
      List<String> urls = new List<String>();

      HttpResponseMessage result = await SearchClient.GetAsync(
        $"{ImageSearchEndPoint}?q={WebUtility.UrlEncode(query)}&safeSearch=Strict&imageType=Photo&color=ColorOnly&count={count}&offset={offset}{(String.IsNullOrEmpty(imageContent) ? "" : "&imageContent=" + imageContent)}");

      result.EnsureSuccessStatusCode();
      String json = await result.Content.ReadAsStringAsync();
      dynamic data = JObject.Parse(json);
      if (data.value == null || data.value.Count <= 0)
        return urls;
      for (Int32 i = 0; i < data.value.Count; i++)
      {
        urls.Add(data.value[i].contentUrl.Value);
      }
      return urls;
    }

    public static async Task<IEnumerable<String>> GetAutoSuggestResults(String query, String market = "en-US")
    {
      List<String> suggestions = new List<String>();
      HttpResponseMessage result = await AutoSuggestionClient.GetAsync(String.Format("{0}/?q={1}&mkt={2}", AutoSuggestionEndPoint, WebUtility.UrlEncode(query), market));

      result.EnsureSuccessStatusCode();

      String json = await result.Content.ReadAsStringAsync();
      dynamic data = JObject.Parse(json);
      if (data.suggestionGroups == null || data.suggestionGroups.Count <= 0 ||
          data.suggestionGroups[0].searchSuggestions == null)
        return suggestions;
      for (Int32 i = 0; i < data.suggestionGroups[0].searchSuggestions.Count; i++)
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
    public static async Task<List<BingNewsEntity>> GetNewsSearchResults(String query, Int32 count = 20, Int32 offset = 0, String market = "zh-CN", String freshness = "Day")
    {
      List<BingNewsEntity> articles = new List<BingNewsEntity>();
      HttpResponseMessage result = await SearchClient.GetAsync(
        $"{NewsSearchEndPoint}/?q={WebUtility.UrlEncode(query)}&count={count}&offset={offset}&mkt={market}&freshness={freshness}");

      result.EnsureSuccessStatusCode();
      String json = await result.Content.ReadAsStringAsync();
      dynamic data = JObject.Parse(json);

      if (data.value == null || data.value.Count <= 0) return articles;
      for (Int32 i = 0; i < data.value.Count; i++)
      {
        BingNewsEntity news = new BingNewsEntity {
          Title = data.value[i].name,
          Url = data.value[i].url,
          Description = data.value[i].description,
          ThumbnailUrl = data.value[i].image?.thumbnail?.contentUrl,
          Provider = data.value[i].provider?[0].name,
          DatePublished = data.value[i].datePublished,
          CateGory = data.value[i].category
        };
        if (!String.IsNullOrEmpty(news.ThumbnailUrl))
        {
          articles.Add(news);
        }
      }
      return articles;
    }
  }
}