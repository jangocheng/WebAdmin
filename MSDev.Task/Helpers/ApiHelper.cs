using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MSDev.Task.Helpers
{
  public class ApiHelper
  {
    private const String Daemon = "https://api.msdev.cc/api/";
    private static String _url = "";

    public ApiHelper(String url)
    {
      _url = Daemon + url;
    }

    public async Task<JsonResult<T>> Get<T>()
    {
      using (HttpClient httpClient = new HttpClient())
      {
        String jsonResult = await httpClient.GetStringAsync(_url);
        JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
        return result;
      }
    }

    public async Task<JsonResult<T>> Post<T>(Object item)
    {
      using (HttpClient httpClient = new HttpClient())
      {

        String stringContent = JsonConvert.SerializeObject(item);
        HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await httpClient.PostAsync(_url, content);
        String jsonResult = await responseMessage.Content.ReadAsStringAsync();
        JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
        return result;
      }

    }

    public async Task<JsonResult<T>> Put<T>(Object item)
    {
      using (HttpClient httpClient = new HttpClient())
      {

        String stringContent = JsonConvert.SerializeObject(item);
        HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await httpClient.PutAsync(_url, content);
        String jsonResult = await responseMessage.Content.ReadAsStringAsync();
        JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
        return result;
      }

    }

    public async Task<JsonResult<T>> Delete<T>()
    {
      using (HttpClient httpClient = new HttpClient())
      {
        HttpResponseMessage responseMessage = await httpClient.DeleteAsync(_url);
        String jsonResult = await responseMessage.Content.ReadAsStringAsync();
        JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
        return result;
      }
    }

  }

  public class JsonResult<T>
  {
    /// <summary>错误代码</summary>
    public Int32 ErrorCode { get; set; }

    /// <summary>消息</summary>
    public String Msg { get; set; }

    /// <summary>数据</summary>
    public T Data { get; set; }

    /// <summary>分页信息</summary>
    public PagerOption PageOption { get; set; }

    /// <summary>返回时间</summary>
    public DateTime DateTime => DateTime.Now.ToLocalTime();
  }

  public class PagerOption
  {
    /// <summary>
    /// 当前页  必传
    /// </summary>
    public Int32 CurrentPage { get; set; }

    /// <summary>
    /// 总条数  必传
    /// </summary>
    public Int32 Total { get; set; }

    /// <summary>
    /// 分页记录数（每页条数 默认每页15条）
    /// </summary>
    public Int32 PageSize { get; set; }

    /// <summary>
    /// 路由地址(格式如：/Controller/Action) 默认自动获取
    /// </summary>
    public String RouteUrl { get; set; }

    /// <summary>
    /// 样式 默认 bootstrap样式 1
    /// </summary>
    public Int32 StyleNum { get; set; }
  }

}