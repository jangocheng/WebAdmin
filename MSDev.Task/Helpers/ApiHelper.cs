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
		private const string BaseDaemon = "https://api.msdev.cc";
		//private const string BaseDaemon = "http://localhost:5000";


		public ApiHelper()
		{

		}

		public async Task<JsonResult<T>> Get<T>(string url)
		{

			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(BaseDaemon);
				string jsonResult = await httpClient.GetStringAsync(url);
				JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
				return result;
			}
		}

		public async Task<JsonResult<T>> Post<T>(string url, object item)
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(BaseDaemon);

				string stringContent = JsonConvert.SerializeObject(item);
				HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");

				HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);
				string jsonResult = await responseMessage.Content.ReadAsStringAsync();

				JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
				return result;
			}

		}

		public async Task<JsonResult<T>> Put<T>(string url, object item)
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(BaseDaemon);

				string stringContent = JsonConvert.SerializeObject(item);
				HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");

				HttpResponseMessage responseMessage = await httpClient.PutAsync(url, content);
				string jsonResult = await responseMessage.Content.ReadAsStringAsync();
				JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
				return result;
			}

		}

		public async Task<JsonResult<T>> Delete<T>(string url)
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(BaseDaemon);

				HttpResponseMessage responseMessage = await httpClient.DeleteAsync(url);
				string jsonResult = await responseMessage.Content.ReadAsStringAsync();
				JsonResult<T> result = JsonConvert.DeserializeObject<JsonResult<T>>(jsonResult);
				return result;
			}
		}

	}

	public class JsonResult<T>
	{
		/// <summary>错误代码</summary>
		public int ErrorCode { get; set; }

		/// <summary>消息</summary>
		public string Msg { get; set; }

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
		public int CurrentPage { get; set; }

		/// <summary>
		/// 总条数  必传
		/// </summary>
		public int Total { get; set; }

		/// <summary>
		/// 分页记录数（每页条数 默认每页15条）
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// 路由地址(格式如：/Controller/Action) 默认自动获取
		/// </summary>
		public string RouteUrl { get; set; }

		/// <summary>
		/// 样式 默认 bootstrap样式 1
		/// </summary>
		public int StyleNum { get; set; }
	}

}