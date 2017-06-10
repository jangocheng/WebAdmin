using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Internal;
using MSDev.DB.Models;
using MSDev.Task.Entities;
using MSDev.Task.Tools;
using Newtonsoft.Json;
using static System.String;

namespace MSDev.Task.Helpers
{
	public class C9Helper
	{
		private const string BeginUrl = "https://channel9.msdn.com/Browse/AllContent?lang=en&lang=zh-cn&lang=zh-tw";

		private const string C9Daemon = "https://channel9.msdn.com/";

		private static readonly HttpClient HttpClient = new HttpClient();
		public C9Helper()
		{
		}

		/// <summary>
		/// 获取 列表分页总数
		/// </summary>
		/// <returns></returns>
		public async Task<int> GetTotalPage()
		{

			int pageNumber = 0;
			var hc = new HttpClient();
			string htmlString = await hc.GetStringAsync(BeginUrl);
			if (IsNullOrEmpty(htmlString)) return pageNumber;
			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(htmlString);

			string totalPage = htmlDoc.DocumentNode
				.SelectSingleNode("//main/div[@class='paging nav holder']//li/span[@class='ellip']")
				.ParentNode.SelectSingleNode(".//a")
				.InnerText; //总页数
			pageNumber = int.Parse(totalPage);
			return pageNumber;
		}

		/// <summary>
		/// 获取视频列表
		/// </summary>
		public async Task<List<C9Article>> GetArticleListAsync(int page = 1)
		{
			var articleList = new List<C9Article>();
			try
			{
				string url = BeginUrl + "&page=" + page;
				string htmlString = await HttpClient.GetStringAsync(url);
				if (!IsNullOrEmpty(htmlString))
				{
					var htmlDoc = new HtmlDocument();
					htmlDoc.LoadHtml(htmlString);
					// 解析获取内容列表
					articleList = htmlDoc.DocumentNode.Descendants("article")
						.Where(n => n.Attributes.Contains("data-api"))
						.Select(n => new C9Article
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
		public C9Video GetPageVideoByUrl(string fullUrl)
		{
			C9Video video = new C9Video();
			try
			{
				var hw = new HtmlWeb();
				// option获取InnerText，需要加以下设置
				HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("option");
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

				var downloadUrls = mainNode.SelectNodes(".//div[@class='download']//select/option")?
					.Select(s => new
					{
						text = s.InnerHtml,
						value = s.Attributes["value"]?.Value
					}).ToList();

				if (downloadUrls != null)
				{
					foreach (var downloadUrl in downloadUrls)
					{
						switch (downloadUrl.text.Trim())
						{
							case "MP3":
								video.Mp3Url = downloadUrl.value;
								break;
							case "Low Quality MP4":
								video.Mp4LowUrl = downloadUrl.value;
								break;
							case "Mid Quality MP4":
								video.Mp4MidUrl = downloadUrl.value;
								break;
							case "High Quality MP4":
								video.Mp4HigUrl = downloadUrl.value;
								break;
							default:
								break;
						}
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
		/// <returns></returns>
		public async Task<C9Video> GetPageVideo(C9Article article, string fullUrl = null)
		{
			var video = new C9Video
			{
				Duration = article.Duration,
				SeriesTitle = article.SeriesTitle,
				SeriesTitleUrl = article.SeriesTitleUrl,
				SourceUrl = article.SourceUrl,
				Title = article.Title,
				ThumbnailUrl = article.ThumbnailUrl
			};
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
				video.Description = mainNode.SelectSingleNode(".//section[@class='ch9tab description']/div[@class='ch9tabContent']")
					.InnerHtml;

				var downloadUrls = mainNode.SelectNodes(".//div[@class='download']//select/option")?
					.Select(s => new
					{
						text = s.InnerHtml,
						value = s.Attributes["value"]?.Value
					}).ToList();

				foreach (var downloadUrl in downloadUrls)
				{
					switch (downloadUrl.text.Trim())
					{
						case "MP3":
						video.Mp3Url = downloadUrl.value;
						break;
						case "Low Quality MP4":
						video.Mp4LowUrl = downloadUrl.value;
						break;
						case "Mid Quality MP4":
						video.Mp4MidUrl = downloadUrl.value;
						break;
						case "High Quality MP4":
						video.Mp4HigUrl = downloadUrl.value;
						break;
						default:
						break;
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
				Log.Write("c9videoGetErrors.txt", url);
				Console.WriteLine($"The Error:{url}");
			}
			return video;
		}
	}
}