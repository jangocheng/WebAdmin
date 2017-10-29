using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;
using System.Linq;
using System.Security.Claims;
using WebAdmin.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MSDev.DB.Entities;
using System.Threading.Tasks;

namespace WebAdmin.Controllers
{
    public class AuthController : Controller
    {


        readonly AppDbContext _context;
        readonly UserManager<User> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly SignInManager<User> _signInManager;
        public AuthController(AppDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.Where(m => m.UserName.Equals(username)|| m.Email.Equals(username)).FirstOrDefault();
            if (user == null)
            {
                ViewBag.Error = "Not Found";
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var identity = new ClaimsIdentity("admin");
                    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, "admin"));
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync("MSDevAdmin", principal).Wait();
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
            HttpContext.SignOutAsync("MSDevAdmin");
            HttpContext.Items.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forbidden()
        {
            return Content("");
        }

        /// <summary>
        /// 初始化管理用户
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IActionResult> InitialUserAsync(string password)
        {
            if (_userManager.Users.Any(m => m.Email.Equals("zpty@outlook.com")))
            {
                return Content("finish");
            }
            var newUser = new User
            {
                Email = "zpty@outlook.com",
                UserName = "NilTor"
            };
            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                //创建role
                var result1 = await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",

                });
                if (result1.Succeeded)
                {
                    var result2 = await _userManager.AddToRoleAsync(newUser, "Admin");
                    if (result2.Succeeded)
                    {
                        return Content("success");
                    }
                    AddErrors(result2);
                }
                AddErrors(result1);
            }
            AddErrors(result);
            return Content(ModelState.Values.First()?.Errors.First()?.ErrorMessage);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
