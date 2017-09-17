using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.Helpers;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models.FormModels.Article;

namespace WebAdmin.Controllers
{
    /// <summary>
    /// 资源管理,如下载资源
    /// </summary>
    public class ArticleController : BaseController
    {
        private readonly AppDbContext _context;
        public ArticleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int p=1)
        {
            int pageSize = 12;
            var blogs = _context.Blog.OrderByDescending(m => m.UpdateTime)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalNumber = _context.Blog.Count();

            SetPage(p, pageSize, totalNumber);
            return View(blogs);
        }


        [HttpGet]
        public IActionResult Write()
        {
            //查询文章分类
            var catalogs = _context.Catalog.Where(m => m.Type == "文章")
                .ToList();
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty,ErrorMessage);
            }
            ViewBag.Catalogs = catalogs;

            return View();
        }

        [HttpPost]
        public IActionResult AddArticle(AddArticleForm article)
        {
            if (ModelState.IsValid)
            {
                if (_context.Blog.Any(m => m.Title.Equals(article.Title)))
                {
                    ErrorMessage = "重复的标题";
                    return RedirectToAction("Write");
                }
                //TODO:作者与状态待完善
                var newBlog = new Blog
                {
                    Id = Guid.NewGuid(),
                    Catalog = _context.Catalog.Find(article.CatalogId),
                    CreatedTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    Title = article.Title,
                    Content = article.Content,
                    Status = "new",
                    AuthorName = "NilTor"
                };

                _context.Blog.Add(newBlog);
                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            SetErrorMessage();

            return RedirectToAction("Write");
        }
    }

}
