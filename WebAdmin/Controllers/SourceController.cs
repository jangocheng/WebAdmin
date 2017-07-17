using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.FormModels.Resource;
using Microsoft.EntityFrameworkCore;

namespace WebAdmin.Controllers
{
    /// <summary>
    /// 资源管理,如下载资源
    /// </summary>
    public class SourceController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SourceController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 下载管理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Download()
        {
            ViewBag.Downloads = _context.Resource
                .Where(m => m.Catalog.Type.Equals("下载"))
                .ToList();

            ViewBag.Catalogs = _context.Catalog.Where(m => m.Type == "下载").ToList();

            return View();
        }

        [HttpGet]
        public IActionResult EditDownload(string id)
        {
            ViewBag.Downloads = _context.Resource
                .Where(m => m.Catalog.Type.Equals("下载"))
                .ToList();

            var resource = _context.Resource
                .Include(m => m.Catalog)
                .SingleOrDefault(m => m.Id == Guid.Parse(id));
            return Json(resource);
            return View(resource);
        }


        [HttpPost]
        public IActionResult UpdateDownload()
        {


            return View();
        }


        /// <summary>
        /// 添加下载资源
        /// </summary>
        /// <param name="resourceForm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddDownload(ResourceForm resourceForm)
        {
            if (ModelState.IsValid)
            {
                if (_context.Resource.Any(m => m.Name == resourceForm.Name && m.Catalog.Id.ToString() == resourceForm.Catalog))
                {
                    return JumpPage("该名称已存在");
                }

                var resource = new Resource
                {
                    AbsolutePath = resourceForm.AbsolutePath,
                    Catalog = _context.Catalog.Find(Guid.Parse(resourceForm.Catalog)),
                    Description = resourceForm.Description,
                    CreatedTime = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Imgurl = resourceForm.IMGUrl,
                    Language = resourceForm.Language,
                    Name = resourceForm.Name,
                    Path = resourceForm.Path,
                    Status = 0,
                    Type = resourceForm.Type,
                    UpdatedTime = DateTime.Now
                };

                _context.Add(resource);
                _context.SaveChanges();
                return RedirectToAction("Download");
            }

            return JumpPage();
        }

        /// <summary>
        /// 删除下载资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DelDownload([FromRoute]string id)
        {
            var resource = _context.Resource.Find(Guid.Parse(id));
            if (resource != null)
            {
                _context.Resource.Remove(resource);
                _context.SaveChangesAsync();
            }

            return RedirectToAction("Download");
        }

    }

}
