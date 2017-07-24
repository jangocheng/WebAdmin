using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MSDev.DB.Entities;
using WebAdmin.FormModels.Catalog;
using AutoMapper;
using MSDev.DB;
using WebAdmin.FormModels.Config;
using static System.String;
using Microsoft.EntityFrameworkCore;

namespace WebAdmin.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ConfigController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Catalog()
        {
            //获取所有目录
            var catalogs = _context.Catalog.ToList();
            ViewBag.Catalogs = catalogs;
            return View();
        }

        [HttpGet]
        public IActionResult EditCatalog(string id)
        {
            if (IsNullOrEmpty(id))
            {
                RedirectToAction("Catalog");
            }
            var catalog = _context.Catalog
                .Include(m => m.TopCatalog)
                .SingleOrDefault(m => m.Id == Guid.Parse(id));

            return View(catalog);
        }

        [HttpPost]
        public IActionResult UpdateCatalog(Catalog catalog)
        {
            if (ModelState.IsValid)
            {
                if (_context.Catalog.Any(m => m.Id == catalog.Id))
                {
                    catalog.UpdatedTime = DateTime.Now;
                    _context.Update(catalog);
                    _context.SaveChanges();

                   return RedirectToAction("Catalog");
                }
            }
            return View("EditCatalog", catalog);
        }

        /// <summary>
        /// 添加目录
        /// </summary>
        /// <param name="catalogForm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddCatalog(CatalogForm catalogForm)
        {
            if (ModelState.IsValid)
            {
                Catalog topCatalog = null;
                if (IsNullOrEmpty(catalogForm.TopCatalog))
                {
                    catalogForm.IsTop = 1;
                }
                else
                {
                    catalogForm.IsTop = 0;
                    topCatalog = _context.Catalog.Find(Guid.Parse(catalogForm.TopCatalog));
                }

                var catalog = new Catalog()
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    IsTop = catalogForm.IsTop,
                    Name = catalogForm.Name,
                    Status = 0,
                    TopCatalog = topCatalog,
                    Type = catalogForm.Type,
                    Value = catalogForm.Value
                };

                _context.Catalog.Add(catalog);
                _context.SaveChanges();
            }
            return RedirectToAction("Catalog", catalogForm);
        }

        [HttpGet]
        public IActionResult Config()
        {
            ViewBag.Configs = _context.Config.ToList();
            return View();
        }


        [HttpPost]
        public IActionResult AddConfig(ConfigForm configForm)
        {
            if (ModelState.IsValid)
            {
                var config = new Config()
                {
                    Name = configForm.Name,
                    Type = configForm.Type,
                    Value = configForm.Value,
                    Id = Guid.NewGuid()
                };

                _context.Config.Add(config);
                _context.SaveChanges();

            }
            return RedirectToAction("Config", configForm);
        }

        /// <summary>
        /// 删除一条配置
        /// </summary>
        /// <returns></returns>
        public IActionResult DelConfig([FromRoute]string id)
        {
            var config = _context.Config.Find(Guid.Parse(id));
            _context.Config.Remove(config);
            _context.SaveChanges();
            return RedirectToAction("Config");
        }
    }

}
