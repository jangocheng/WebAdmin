using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MSDev.Task.Entities;
using Newtonsoft.Json;

namespace MSDev.Task.Helpers
{
	class C9Helper
	{

		static string beginUrl = "https://channel9.msdn.com/Browse/AllContent?lang=en&lang=zh-cn&lang=zh-tw";
		public C9Helper()
		{

		}
		/// <summary>
		/// 获取视频列表
		/// </summary>
		public async Task<bool> GetArticleListAsync()
		{
			var hc = new HttpClient();
			try
			{
				string htmlString = await hc.GetStringAsync(beginUrl);
				if (!String.IsNullOrEmpty(htmlString))
				{
					var htmlDoc = new HtmlDocument();

					htmlDoc.LoadHtml(htmlString);

					string totalPage = htmlDoc.DocumentNode.SelectSingleNode("//main/div[@class='paging nav holder']//li/span[@class='ellip']")
					.ParentNode.SelectSingleNode(".//a")
					.InnerText;//总页数

					// 解析获取内容列表
					var articleList = htmlDoc.DocumentNode.Descendants("article")
						.Where(n => n.Attributes.Contains("data-api"))
					.Select(n =>
						 new C9ArticleEntity
						 {
							 SeriesTitle = n.SelectSingleNode(".//div[@class='seriesTitle']/a")?.InnerText,
							 SeriesTitleUrl = n.SelectSingleNode(".//div[@class='seriesTitle']/a")?.GetAttributeValue("href", String.Empty),
							 Title = n.SelectSingleNode(".//header/h3/a")?.InnerText,
							 SourceUrl = n.SelectSingleNode(".//header/h3/a")?.GetAttributeValue("href", String.Empty),
							 ThumbnailUrl = n.SelectSingleNode(".//a/img")?.GetAttributeValue("src", String.Empty),
							 Duration = n.SelectSingleNode(".//a/time")?.GetAttributeValue("datetime", String.Empty)
						 }
					)
					.ToList();

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Source, ex.Message);
				throw;

			}
			return false;
		}
	}
}