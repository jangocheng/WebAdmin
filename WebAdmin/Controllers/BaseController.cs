using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSDev.Task.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Controllers
{
	[Route("[controller]")]
	[Authorize(Policy = "admin")]
	public class BaseController : Controller
	{
		protected ApiHelper _aipHelper;

		public BaseController()
		{

		}
		public BaseController(ApiHelper apiHelper)
		{
			_aipHelper = apiHelper;
		}

	}
}
