using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAdmin.Controllers
{
	public class AuthController : Controller
	{

		public AuthController()
		{

		}

		[HttpGet]
		public IActionResult Login()
		{


			return View();
		}


		[HttpPost]
		public IActionResult Login(string username, string password)
		{

			if (username == "admin" && password == "MSDev.cc")
			{
				var identity = new ClaimsIdentity("admin");
				identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
				identity.AddClaim(new Claim(ClaimTypes.Name, "admin"));

				var principal = new ClaimsPrincipal(identity);
				HttpContext.Authentication.SignInAsync("MSDevAdmin", principal);

				HttpContext.Items["username"] = username;
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewBag.Error = "Wrong username or password";
			}
			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Authentication.SignOutAsync("MSDevAdmin");
			HttpContext.Items.Clear();
			return RedirectToAction("Index", "Home");

		}

		public IActionResult Forbidden()
		{
			return View();
		}
	}
}
