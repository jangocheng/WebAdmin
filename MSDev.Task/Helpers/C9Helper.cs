using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MSDev.Task.Entities;
using Newtonsoft.Json;
using static System.String;

namespace MSDev.Task.Helpers
{
	public class C9Helper
	{
		private static string _beginUrl = "https://channel9.msdn.com/Browse/AllContent?lang=en&lang=zh-cn&lang=zh-tw";

		public C9Helper()
		{

		}


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
		public async Task<List<C9ArticleEntity>> GetArticleListAsync(int page = 1)
		{
			var articleList = new List<C9ArticleEntity>();
			var hc = new HttpClient();

			_beginUrl = _beginUrl + "?page=" + page;
			try
			{
				string htmlString = await hc.GetStringAsync(_beginUrl);
				if (!IsNullOrEmpty(htmlString))
				{
					var htmlDoc = new HtmlDocument();
					htmlDoc.LoadHtml(htmlString);
					// 解析获取内容列表
					articleList = htmlDoc.DocumentNode.Descendants("article")
						.Where(n => n.Attributes.Contains("data-api"))
						.Select(n => new C9ArticleEntity
						{
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
				Console.WriteLine(ex.Source, ex.Message);
				throw;

			}
			return articleList;

		}
	}
}