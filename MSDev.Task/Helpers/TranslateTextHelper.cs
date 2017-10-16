using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using MSDev.Work.Models;
using MSDev.Work.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            string seperator = "<!--divider-->";

            if (content.Length > 5000)
            {
                //分解 content
                content = addSeperator(content, 1);
            }
            // 内部方法，添加分隔符
            string addSeperator(string str, int tagLevel)
            {
                if (tagLevel > 5) return str; //避免无限递归

                var result = "";
                var tag = $"</h{tagLevel}>";
                if (str.Contains(tag))
                {
                    str = str.Replace(tag, tag + seperator);
                    var contentParts = str.Split(tag);
                    foreach (var item in contentParts)
                    {
                        var row = item + tag;
                        if (row.Length >= 5000)
                        {
                            row = addSeperator(row, tagLevel + 1);
                        }
                        result += row;
                    }
                    return result;
                }
                else
                {
                    result = addSeperator(str, tagLevel + 1);
                }
                return result;
            }

            var contentArray = content.Split(seperator);
            string translation = "";//最后翻译结果
            foreach (var item in contentArray)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                translation += GetBingTranslateAsync(item).Result;
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

        public async Task<string> GetBingTranslateAsync(string content)
        {
            string result = "";

            Log.Write("translate.txt", "source:" + content);
            if (content == null)
            {
                return default;
            }
            var requestBody = new List<BingTranslateRequest>()
            {
                new BingTranslateRequest
                {
                    Text = content,
                    SourceLanguage = "en",
                    TargetLanguage = "zh-Hans"
                }
            };
            using (var hc = new HttpClient())
            {
                try
                {
                    hc.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "mtstkn=zjY%2FeGV9RQpJ%2Fb5zHXmn%2FwDrkC3fmMTQXW1jhbAMg4NqrZ3Q%2BCtyMCEtkorJgErv; MUID=36C006E0669A654423240C47629A637D; SRCHD=AF=NOFORM; SRCHUID=V=2&GUID=48E5753D244145AF83BA59BFCB948ED9; SRCHUSR=DOB=20170620; MUIDB=36C006E0669A654423240C47629A637D; _IFAV=COUNT=NaN; ULC=H=143E5|1:1&T=143E5|1:1; _SS=SID=0794CF56E29F693D0826C44CE33E684D; _EDGE_S=SID=0794CF56E29F693D0826C44CE33E684D; MicrosoftApplicationsTelemetryDeviceId=9486e28c-bd2a-64f3-20cd-4e2289a04ab2; MicrosoftApplicationsTelemetryFirstLaunchTime=1507873173107; WLID=09wu9p7hOyscyLh8jjXDpTyvU59VrzyOdy65bmcs1SPJSpfKqHV75d+Di3Qj3UmhJQNoPjXEHvvFNrwIhboIuKzI/pcCuh0EslSRyg6wj/s=; SRCHHPGUSR=CW=1348&CH=813&DPR=2&UTC=480&WTS=63643469988; KievRPSAuth=FABSARRaTOJILtFsMkpLVWSG6AN6C/svRwNmAAAEgAAACK2pGNUp/oQTEAG3uxqfCOw%2BWMN9jdJBkcfEg6p9nZAaDAPUfjygE%2B3%2BAoU2Z/hWRSFYCgww9tQZI91OwfGkNStUrNY6axW%2BeYRbt8iYJsq9QE4dPORIC7VbQfuLcAr9qC8BdnthLwqzm%2BNrmjSs%2B0Vl8OnPmAJ6ge8ayH6rC%2BcLYDlMVhiFExVZA/O1kNtv5mKAqSphmNwTFAoB1e/xk22t70rUNPiarFjA/wb4YO0bO/R3bfChbha6uiuZcVJ2UdX3lVUxGj0NVumKOm5v00w%2BFP4nLmiKmO%2B%2BCrv8dKTMETP7RH8Jlt3afs7bQIYUuYrNkuuhL96bxMtKq5Mauase0mWcDSVb0BMHcTKONxSExjNIb/f59d1Q8BQAbbc/LYjlCDKHPyjkp4IrPHTYYbc%3D; PPLState=1; ANON=A=BAD034877E3773F0C11EE3C0FFFFFFFF&E=1408&W=1; NAP=V=1.9&E=13ae&C=FHVwH60y3BvccG8R2d1IBs5Y4F4rx64OlTNWCRFkTqBBglUU1PfUxQ&W=1; SNRHOP=I=&TS=; _U=1FBIrhwNdfcoMbdijf7xN8PKj81oPmBG-hv46BYL7fA6X20-PANHcSDkkNGHgM7QD7voL4f2AQP7eusLniNyTLl-nhEsjQh8aCblOhz1YDekFnaCnu5etFF57DsjSIrnG; WLS=C=fc6e0c59a6c0a524&N=zpty; srcLang=-; smru_list=; sourceDia=en-US; destLang=zh-CHS; dmru_list=da%2Czh-CHS; destDia=zh-CN");

                    hc.DefaultRequestHeaders.TryAddWithoutValidation("Host", "translator.microsoft.com");
                    hc.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    hc.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://translator.microsoft.com");
                    //hc.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.bing.com/translator");

                    var body = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var response = await hc.PostAsync("https://translator.microsoft.com/neural/api/translator/translate", body);
                    result = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("result:" + result);
                    var rspObject = JsonConvert.DeserializeObject<BingTranslateResponse>(result);
                    result = rspObject.ResultNMT;
                    Log.Write("translate.txt", "Translate:" + result);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Source + e.Message);
                    Log.Write("translateError.txt", e.Source + e.Message);

                }
            }
            return result;
        }
    }
}
