using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSDev.Task.Helpers;

namespace WebAdmin.Controllers
{
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
