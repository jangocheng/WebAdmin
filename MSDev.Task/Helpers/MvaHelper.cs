using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using MSDev.DB.Entities;
using MSDev.Task.Entities;
using Newtonsoft.Json;
using HtmlAgilityPack;
using MSDev.Task.Tools;

namespace MSDev.Task.Helpers
{
    public class MvaHelper
    {
        private const string BeginUrl = "https://api-mlxprod.microsoft.com/sdk/search/v1.0/5/courses";

        private const string MvaDaemon = "https://mva.microsoft.com/";
        // 请求json字符串
        private const string ReqStr = @"{""SelectCriteria"":[{""SelectOnField"":""LCID"",""SelectTerm"":""1033"",""SelectMatchOption"":2},{""SelectOnField"":""LCID"",""SelectTerm"":""1028"",""SelectMatchOption"":2},{""SelectOnField"":""LCID"",""SelectTerm"":""2052"",""SelectMatchOption"":2}],""DisplayFields"":[],""SortOptions"":[{""SortOnField"":""PublishedTime"",""SortOrder"":1}],""SearchKeyword"":"""",""UILangaugeCode"":2052,""UserLanguageCode"":2052}";



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
        public async Task<List<MvaVideos>> GetMvaVideos(int skip = 0, int number = 100)
        {
            var list = new List<MvaVideos>();
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
                    var mvaVideo = new MvaVideos()
                    {
                        Id = Guid.NewGuid(),
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


        public async Task<List<MvaDetails>> GetMvaDetails(string url)
        {
            string apimlxprod = "https://api-mlxprod.microsoft.com/services/products/anonymous/17809";

            var list = new List<MvaDetails>();
            HttpClient hc = new HttpClient();

            string courseInfoUrl =await hc.GetStringAsync(apimlxprod);
            courseInfoUrl = JsonConvert.DeserializeObject<string>(courseInfoUrl);
            courseInfoUrl = courseInfoUrl + "/imsmanifestlite.json";
            Console.WriteLine(courseInfoUrl);
            string courseInfo = await hc.GetStringAsync(courseInfoUrl);
            Log.Write("info.json", courseInfo);

            string htmlString = await hc.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);
            var info = htmlDoc.DocumentNode.SelectSingleNode(".//main[@role='main']//section[@id='coursePlayer']//div[@id='info-tab-container']//div[@id='course-info-container']");

            string detailDescription = info.SelectSingleNode(".//div[@id='overview']/div[@class='accordian-container overview-container-height']")?.InnerHtml;

            var mvaDetails = info.SelectSingleNode(".//div[@id='syllabus']//div[@id='accordian-container']");

            Console.WriteLine(mvaDetails.InnerHtml);
            //    .Select(m => new MvaDetails
            //    {
            //        Title = m.SelectSingleNode(".//div[@class='corse-item-name']").Attributes["title"]?.Value,
            //        SourceUrl = url + m.Attributes["id"]?.Value.Replace("activity-", "?l="),
            //        Duration = DateTime.Parse(m.SelectSingleNode(".//div[@class='corse-item-progress']//span[@class='corse-item-span']")?.InnerText),

            //    });

            //foreach (var detail in mvaDetails)
            //{
            //    if (detail.Title.Contains("学习手册"))
            //    {
            //        detail.Title = string.Empty;
            //        continue;
            //    }

            //    var htmlDoc1 = new HtmlDocument();
            //    htmlDoc1.LoadHtml(detail.SourceUrl);
            //    var content = htmlDoc1.DocumentNode.SelectNodes(".//div[@id='video-player-wrapper']//video[@class='pf-video']//source[@type='video/mp4']");
            //    detail.LowDownloadUrl = content.Where(s => s.Attributes["videoMode"].Equals("360p"))?.First().Attributes["src"]?.Value;
            //    detail.MidDownloadUrl = content.Where(s => s.Attributes["videoMode"].Equals("540p"))?.First().Attributes["src"]?.Value;
            //    detail.HighDownloadUrl = content.Where(s => s.Attributes["videoMode"].Equals("720p"))?.First().Attributes["src"]?.Value;


            //    Console.WriteLine(JsonConvert.SerializeObject(detail));
            //    list.Add(detail);
            //}

            return list;

        }



    }
}