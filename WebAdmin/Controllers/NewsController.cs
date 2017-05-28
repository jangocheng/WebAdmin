using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MSDev.Task.Helpers;
using MSDev.Task.Models;
using Newtonsoft.Json;

namespace WebAdmin.Controllers
{

	public class NewsController : BaseController
	{
		readonly ApiHelper _apiHelper;
		public NewsController(ApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> BingNews(int id)
		{
			JsonResult<List<BingNews>> re = await _apiHelper.Get<List<BingNews>>($"/api/manage/bingnews/pagelist?p={id}");
			if (re.ErrorCode == 0)
			{
				ViewBag.ListData = re.Data;
				re.PageOption.RouteUrl = "/news/bingnews";
				ViewBag.Pager = re.PageOption;
				Console.WriteLine(JsonConvert.SerializeObject(re.PageOption));
			}
			return View();
		}
	}
}