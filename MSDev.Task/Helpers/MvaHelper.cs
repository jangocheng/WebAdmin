using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Internal;
using MSDev.DB.Entities;
using MSDev.Work.Entities;
using MSDev.Work.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebAdmin.DB.Utils;

namespace MSDev.Work.Helpers
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



        public async Task<(string, List<MvaDetails>)> GetMvaDetails(MvaVideos video)
        {
            //TODO:此处需要处理版本号
            string apimlxprod = "https://api-mlxprod.microsoft.com/services/products/anonymous/" + video.MvaId;

            string url = video.SourceUrl;
            string courseInfoUrl = "";

            var list = new List<MvaDetails>();
            try
            {
                HttpClient hc = new HttpClient();
                //静态页面分析
                string htmlString = await hc.GetStringAsync(url);
                string version = "1.0.0.0";//默认course版本号
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlString);

                version = StringTools.GetRow(htmlString, "courseVersion");
                version = version.Substring(0, version.IndexOf(","));
                version = version.Replace("courseVersion:", string.Empty);
                version = version.Replace("'", string.Empty);
                version = version.Trim();

                string languageId = StringTools.GetRow(htmlString, "languageId");
                languageId = languageId.Substring(0, languageId.IndexOf(","));
                languageId = languageId.Replace("languageId:", string.Empty);
                languageId = languageId.Replace("'", string.Empty);
                languageId = languageId.Trim();

                apimlxprod += $"?version={version}&languageId={languageId}";

                var info = htmlDoc.DocumentNode.SelectSingleNode(".//main[@role='main']//section[@id='coursePlayer']//div[@id='info-tab-container']//div[@id='course-info-container']");

                string detailDescription = info.SelectSingleNode(".//div[@id='overview']/div[@class='accordian-container overview-container-height']")?.InnerHtml;
                detailDescription = detailDescription ?? "无";
               
                string mlxprodStaticUrl = await hc.GetStringAsync(apimlxprod);

                //取课程内容
                mlxprodStaticUrl = JsonConvert.DeserializeObject<string>(mlxprodStaticUrl);
                courseInfoUrl = mlxprodStaticUrl + "/imsmanifestlite.json";
                string courseInfo = await hc.GetStringAsync(courseInfoUrl);
                courseInfo = courseInfo.Replace("@", "_");

                //解析课程内容，获取课程id,名称，时间等
                var mvaCourseInfo = JsonConvert.DeserializeObject<MvaDetailInfoEntity>(courseInfo);
                var courseItems = mvaCourseInfo.manifest.organizations.organization.First().item
                    .Select(m => m.item).ToList();

                int sequence = 1;
                foreach (var courses in courseItems)
                {
                    if (courses == null) continue;
                    foreach (var course in courses)
                    {
                        var mvaDetail = new MvaDetails();
                        if (course.resource.metadata.learningresourcetype.Equals("Video"))
                        {
                            mvaDetail.Id = Guid.NewGuid();
                            mvaDetail.MvaId = course._identifier;
                            mvaDetail.Title = course.title;
                            if (DateTime.TryParse(course.resource?.metadata?.duration, out var duration))
                            {
                                mvaDetail.Duration = duration;
                            }

                            mvaDetail.SourceUrl = video.SourceUrl + "?l=" + course._identifier;
                            mvaDetail.MvaVideo = video;
                            mvaDetail.Status = StatusType.Edit;
                            mvaDetail.Sequence = sequence;
                            mvaDetail.CreatedTime = DateTime.Now;
                            mvaDetail.UpdatedTime = DateTime.Now;
                            list.Add(mvaDetail);
                            sequence++;
                        }
                        continue;
                    }
                }
                //根据课程id，获取下载地址
                foreach (var mvaDetail in list)
                {
                    var downloadUrl = mlxprodStaticUrl + "/content/content_" + mvaDetail.MvaId + "/videosettings.xml";
                    string xmlString = await hc.GetStringAsync(downloadUrl);
                    if (xmlString != null)
                    {
                        var xmlDoc = XDocument.Parse(xmlString);
                        var downloadList = xmlDoc.Root.Element("PlaylistItems").Element("PlaylistItem")?.Elements("MediaSources")
                            .Where(m => m.Attribute("videoType").Value.Equals("progressive"))
                            .First()?.Elements("MediaSource");

                        mvaDetail.LowDownloadUrl = downloadList.Where(m => m.Attribute("videoMode").Value.Equals("360p")).First()?.Value;
                        mvaDetail.MidDownloadUrl = downloadList.Where(m => m.Attribute("videoMode").Value.Equals("540p")).First()?.Value;
                        mvaDetail.HighDownloadUrl = downloadList.Where(m => m.Attribute("videoMode").Value.Equals("720p")).First()?.Value;
                    }

                }

                return (detailDescription, list);
            }
            catch (Exception e)
            {
                Log.Write("mvaDetailErrors.txt", e.Source + e.Message + e.InnerException?.Message);
                Console.WriteLine(e.Source + e.Message);
                return default;
            }
        }
    }
}