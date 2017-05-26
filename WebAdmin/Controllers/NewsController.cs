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

		[HttpGet]
		public async Task<IActionResult> BingNews()
		{
			JsonResult<List<BingNews>> re = await _apiHelper.Get<List<BingNews>>("/api/manage/bingnews/pagelist");
			if (re.ErrorCode == 0)
			{
				ViewBag.ListData = re.Data;
				ViewBag.Page = re.PageOption;

				Console.WriteLine(JsonConvert.SerializeObject(re.PageOption));
			}
			return View();
		}
	}
}