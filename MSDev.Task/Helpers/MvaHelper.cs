using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using MSDev.DB.Models;
using MSDev.Task.Entities;
using Newtonsoft.Json;

namespace MSDev.Task.Helpers
{
	public class MvaHelper
	{
		private const string BeginUrl = "https://api-mlxprod.microsoft.com/sdk/search/v1.0/5/courses";

		private const string MvaDaemon = "https://mva.microsoft.com/";
		// 请求json字符串
		private const string ReqStr = @"{""SelectCriteria"":[{""SelectOnField"":""LCID"",""SelectTerm"":""1028"",""SelectMatchOption"":2},{""SelectOnField"":""LCID"",""SelectTerm"":""2052"",""SelectMatchOption"":2},{""SelectOnField"":""LCID"",""SelectTerm"":""1033"",""SelectMatchOption"":2}],""DisplayFields"":[],""SortOptions"":[{""SortOnField"":""Relevance"",""SortOrder"":1}],""SearchKeyword"":"""",""UILangaugeCode"":2052,""UserLanguageCode"":2052}";

		private static readonly HttpClient HttpClient = new HttpClient();
		public MvaHelper()
		{

		}

		/// <summary>
		/// 获取mva总数
		/// </summary>
		/// <returns></returns>
		public async Task<int> GetTotalNumberAsync()
		{
			HttpResponseMessage result = await HttpClient.PostAsync(BeginUrl + "?$skip=0&$top=1", new StringContent(ReqStr, Encoding.UTF8, "application/json"));
			string jsonResult = await result.Content.ReadAsStringAsync();

			int number = JsonConvert.DeserializeObject<MvaApi>(jsonResult).TotalResultCount;

			return number;

		}
		/// <summary>
		/// 获取一定数量的视频
		/// </summary>
		/// <param name="skip">偏移量</param>
		/// <param name="number">总数</param>
		/// <returns></returns>
		public async Task<List<MvaVideo>> GetMvaVideos(int skip = 0, int number = 100)
		{
			var list = new List<MvaVideo>();
			HttpResponseMessage result =
				await HttpClient.PostAsync(BeginUrl + $"?$skip={skip}&$top={number}", new StringContent(ReqStr, Encoding.UTF8, "application/json"));
			string jsonResult = await result.Content.ReadAsStringAsync();
			List<MvaEntity> results = JsonConvert.DeserializeObject<MvaApi>(jsonResult).Results;
			try
			{
				foreach (MvaEntity mvaEntity in results)
				{
					var regex = new Regex(@"[^a-zA-Z\d\s]+\s*");
					var sourceUrl = regex.Replace(mvaEntity.CourseName, "");
					sourceUrl = sourceUrl.Replace(" ", "-");
					sourceUrl += "-" + mvaEntity.Id;
					sourceUrl = MvaDaemon + mvaEntity.LanguageCode + "/training-courses/" + sourceUrl;
					var mvaVideo = new MvaVideo()
					{
						MvaId = mvaEntity.Id,
						SourceUrl = sourceUrl,
						Title = mvaEntity.CourseName,
						CourseLevel = mvaEntity.CourseLevel,
						LanguageCode = mvaEntity.LanguageCode,
						CourseNumber = mvaEntity.CourseNumber,
						Description = mvaEntity.CourseShortDescription,
						CourseDuration = mvaEntity.CourseDuration,
						CourseImage = mvaEntity.CourseImage,
						CourseStatus = mvaEntity.CourseStatus,
						ProductPackageVersionId = mvaEntity.ProductPackageVersionId,
						Tags = mvaEntity.Tags,
						Technologies = mvaEntity.Technologies.Join(),
						Author = mvaEntity.AuthorInfo.Select(m => m.DisplayName).ToList().Join(),
						AuthorCompany = mvaEntity.AuthorInfo.Select(m => m.Company).ToList().Join(),
						AuthorJobTitle = mvaEntity.AuthorInfo.Select(m => m.JobTitle).ToList().Join(),
						CreatedTime = mvaEntity.PublishedTime,
						UpdatedTime = mvaEntity.LastUpdated
					};

					list.Add(mvaVideo);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return list;
		}


	}
}