using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace MSDev.Work.Helpers
{
    public class TranslateTextHelper
    {

        static AzureAuthTokenHelper _tokenHelper;
        public TranslateTextHelper(string subscriptKey)
        {
            _tokenHelper = new AzureAuthTokenHelper(subscriptKey);
        }

        /// <summary>
        /// 文本翻译
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string TranslateText(string content)
        {

            //处理content
            string seperator = "</h3>";
            if (content.Contains("</h2>"))
            {
                seperator = "</h2>";
            }
            if (content.Contains("</h1>"))
            {
                seperator = "</h1>";
            }
            var contentParts = content.Split(seperator);
            for (int i = 0; i < contentParts.Length - 1; i++)
            {
                contentParts[i] = contentParts[i] + seperator;
            }
            string translation = "";//最后翻译结果
            foreach (var item in contentParts)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                translation += GetTranslate(item);
            }
            return translation;
        }



        public string GetTranslate(string content, string from = "en", string to = "zh-CHS")
        {
            string result = "";
            string authToken = _tokenHelper.GetAccessToken();
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(content) + "&from=" + from + "&to=" + to;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            try
            {
                using (WebResponse response = httpWebRequest.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                    result += (string)dcs.ReadObject(stream);
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default;
        }


        public async Task<string> GetTranslateArrayAsync(string[] sourceTexts, string from = "en", string to = "zh-CHS")
        {
            string result = "";
            string authToken = _tokenHelper.GetAccessToken();
            var uri = "https://api.microsofttranslator.com/v2/Http.svc/TranslateArray";
            var body = "<TranslateArrayRequest>" +
                           "<AppId />" +
                           "<From>{0}</From>" +
                           "<Options>" +
                           " <Category xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\" />" +
                               "<ContentType xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">{1}</ContentType>" +
                               "<ReservedFlags xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\" />" +
                               "<State xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\" />" +
                               "<Uri xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\" />" +
                               "<User xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\" />" +
                           "</Options>" +
                           "<Texts>" +
                               "{2}" +
                           "</Texts>" +
                           "<To>{3}</To>" +
                       "</TranslateArrayRequest>";
            string textsString = "";
            foreach (var item in sourceTexts)
            {
                textsString += $"<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">{item}</string>";
            }
            string requestBody = string.Format(body, from, "text/html", textsString, to);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "text/xml");
                request.Headers.Add("Authorization", authToken);
                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        Console.WriteLine("Request status is OK. Result of translate array method is:");
                        var doc = XDocument.Parse(responseBody);
                        var ns = XNamespace.Get("http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2");
                        var sourceTextCounter = 0;
                        foreach (XElement xe in doc.Descendants(ns + "TranslateArrayResponse"))
                        {
                            foreach (var node in xe.Elements(ns + "TranslatedText"))
                            {
                                Console.WriteLine("\n\nSource text: {0}\nTranslated Text: {1}", sourceTexts[sourceTextCounter], node.Value);
                                result += node.Value;
                            }
                            sourceTextCounter++;
                        }
                        break;
                    default:
                        Console.WriteLine("Request status code is: {0}.", response.StatusCode);
                        Console.WriteLine("Request error message: {0}.", responseBody);
                        break;
                }
            }
            return result;
        }
    }
}
