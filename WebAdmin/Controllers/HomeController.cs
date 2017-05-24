using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAdmin.Controllers
{
	public class HomeController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}



		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
