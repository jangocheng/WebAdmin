using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

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
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public string TranslateText(string content, string from = "en", string to = "zh-cn")
        {
            string authToken = _tokenHelper.GetAccessToken();

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

                string uri = "https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(item) + "&from=" + from + "&to=" + to;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers.Add("Authorization", authToken);
                try
                {
                    using (WebResponse response = httpWebRequest.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    {
                        DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                        translation += (string)dcs.ReadObject(stream);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return translation;
        }
    }
}
