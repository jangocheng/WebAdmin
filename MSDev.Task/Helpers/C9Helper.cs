using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Internal;
using MSDev.DB.Models;
using MSDev.Task.Entities;
using Newtonsoft.Json;
using static System.String;

namespace MSDev.Task.Helpers
{
	public class C9Helper
	{
		private static string _beginUrl = "https://channel9.msdn.com/Browse/AllContent?lang=en&lang=zh-cn&lang=zh-tw";

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
			string htmlString = await hc.GetStringAsync(_beginUrl);
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
			_beginUrl = _beginUrl + "?page=" + page;
			try
			{
				string htmlString = await HttpClient.GetStringAsync(_beginUrl);
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


		public C9Video GetPageVideo(C9Article article)
		{
			var video = new C9Video();



			string url = C9Daemon + article.SourceUrl;
			var hw = new HtmlWeb();
			HtmlDocument htmlDoc = hw.Load(url);
			HtmlNode mainNode = htmlDoc.DocumentNode.SelectSingleNode(".//main[@role='main']");

			video.Author = mainNode.SelectSingleNode(".//div[@class='authors']")?.Descendants("a")?.Select(s => s.InnerText)
				.ToArray().Join();


			video.Language = mainNode.SelectSingleNode(".//div[@class='itemHead holder' and @dir='ltr']")?
				.GetAttributeValue("lang", Empty);
			video.Description = mainNode.SelectSingleNode(".//section[@class='ch9tab description']/div[@class='ch9tabContent']")
				.InnerHtml;




			return video;

		}
	}
}