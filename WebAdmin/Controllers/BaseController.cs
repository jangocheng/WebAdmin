using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSDev.Task.Helpers;

namespace WebAdmin.Controllers
{
	[Authorize(Policy = "admin")]
	public class BaseController : Controller
	{

		public BaseController()
		{

		}

	}
}
