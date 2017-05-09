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
    //private const String BaseDaemon = "https://api.msdev.cc";
    private const String BaseDaemon = "http://localhost:5000";


    public ApiHelper()
    {

    }

    public async Task<JsonResult<T>> Get<T>(String url)
    {

      using (HttpClient httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri(BaseDaemon);
        String jsonResult = await httpClient.GetStringAsync(url);
         JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
        return result;
      }
    }

    public async Task<JsonResult<T>> Post<T>(String url, Object item)
    {
      using (HttpClient httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri(BaseDaemon);

        String stringContent = JsonConvert.SerializeObject(item);
        HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);
        String jsonResult = await responseMessage.Content.ReadAsStringAsync();

        JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
        return result;
      }

    }

    public async Task<JsonResult<T>> Put<T>(String url, Object item)
    {
      using (HttpClient httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri(BaseDaemon);

        String stringContent = JsonConvert.SerializeObject(item);
        HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await httpClient.PutAsync(url, content);
        String jsonResult = await responseMessage.Content.ReadAsStringAsync();
        JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
        return result;
      }

    }

    public async Task<JsonResult<T>> Delete<T>(String url)
    {
      using (HttpClient httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri(BaseDaemon);

        HttpResponseMessage responseMessage = await httpClient.DeleteAsync(url);
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