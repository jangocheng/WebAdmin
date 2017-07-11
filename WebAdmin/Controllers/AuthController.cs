using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using WebAdmin.Helpers;
using Newtonsoft.Json;

namespace WebAdmin.Controllers
{
    public class AuthController : Controller
    {


        readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.AspNetUsers.Where(m => m.Email.Equals(username)).FirstOrDefault();
            if (user == null)
            {
                ViewBag.Error = "Not Found";
            }
            else
            {
                var userRole = _context.AspNetUserRoles.Where(m => m.UserId == user.Id).Include(m => m.Role).FirstOrDefault();

                System.Console.WriteLine(JsonConvert.SerializeObject(userRole));
                if (userRole?.Role.Name == "Admin" && PasswordHelper.VerifyHashedPassword(user.PasswordHash, password))
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
            return Content("");
        }
    }
}
