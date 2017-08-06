using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.Helpers;

namespace WebAdmin.Controllers
{

    public class NewsController : BaseController
    {
        private AppDbContext _context;
        public NewsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BingNews(int p = 1)
        {

            int pageSize = 15;
            var newsList = _context.BingNews
                .OrderByDescending(m => m.UpdatedTime)
                .Skip((p - 1) * pageSize).Take(pageSize)
                .ToList();
            int totalNumber = _context.BingNews.Count();

            ViewBag.ListData = newsList;

            var pageOption = new MyPagerOption()
            {
                CurrentPage = p,
                PageSize = pageSize,
                RouteUrl = "/News/BingNews",
                Total = totalNumber
            };
            ViewBag.Pager = pageOption;

            return View();
        }

        [HttpPost]
        public IActionResult DelNews(string id)
        {
            BingNews news = _context.BingNews.Find(Guid.Parse(id));
            _context.BingNews.Remove(news);
            var re = _context.SaveChanges();
            if (re > 0)
            {
                return JsonOk(re);
            }
            return JsonFailed();

        }

    }
}