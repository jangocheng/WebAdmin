using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.Helpers;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models.FormModels.Article;
using System.Security.Claims;

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

        public IActionResult Index(int p = 1, string series = "")
        {
            int pageSize = 12;
            var blogs = _context.Blog.OrderByDescending(m => m.CreatedTime)
                .Where(m => m.Catalog.Value.Equals(series) || series == "")
                .Include(m => m.Catalog)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalNumber = _context.Blog.Count();

            ViewBag.Catalogs = _context.Catalog
                .Where(m => m.Type.Equals("文章"))
                .ToList();

            SetPage(p, pageSize, totalNumber);
            return View(blogs);
        }


        public IActionResult Write()
        {
            //查询文章分类
            var catalogs = _context.Catalog.Where(m => m.Type == "文章")
                .ToList();
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            ViewBag.Catalogs = catalogs;

            return View();
        }


        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (_context.Blog.Any(m => m.Id.Equals(id)))
            {
                var blog = _context.Blog
                    .Where(m => m.Id == id)
                    .Include(m => m.Catalog)
                    .FirstOrDefault();

                ViewBag.Catalogs = _context.Catalog.Where(m => m.Type.Equals("文章"))
                    .ToList();
                return View(blog);
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public IActionResult UpdateArticle(Blog blog, string CatalogId)
        {
            if (ModelState.IsValid)
            {
                if (blog.Id == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (_context.Blog.Any(m => m.Id == blog.Id))
                {
                    blog.Catalog = _context.Catalog.Find(Guid.Parse(CatalogId));
                    blog.Status = StatusType.Edit;
                    _context.Update(blog);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            SetErrorMessage();
            return RedirectToAction(nameof(Edit), new
            {
                id = blog.Id
            });

        }

        public IActionResult DeleteArticle(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (_context.Blog.Any(m => m.Id.Equals(id)))
            {
                var oldBlog = _context.Blog.Find(id);
                _context.Blog.Remove(oldBlog);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddArticle(AddArticleForm article)
        {
            if (ModelState.IsValid)
            {
                if (_context.Blog.Any(m => m.Title.Equals(article.Title)))
                {
                    ErrorMessage = "重复的标题";
                    return RedirectToAction(nameof(Write));
                }
                //TODO:作者与状态待完善
                var newBlog = new Blog
                {
                    Id = Guid.NewGuid(),
                    Catalog = _context.Catalog.Find(article.CatalogId),
                    CreatedTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    Title = article.Title,
                    Description = article.Description,
                    Tags = article.Tags,
                    Content = article.Content,
                    Status = StatusType.New,
                    AuthorName = User.Claims.Where(c => c.Type == ClaimTypes.Name).First().Value
                };

                _context.Blog.Add(newBlog);
                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            SetErrorMessage();

            return RedirectToAction(nameof(Write));
        }
        /// <summary>
        /// 发布文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult PublishArticle(string id)
        {
            var article = _context.Blog.Find(Guid.Parse(id));
            if (article == null)
            {
                return NotFound();
            }
            article.Status = StatusType.Publish;
            _context.Update(article);
            if (_context.SaveChanges() > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            SetErrorMessage();
            return RedirectToAction(nameof(Index));
        }



    }

}
