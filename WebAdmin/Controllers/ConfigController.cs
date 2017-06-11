using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using MSDev.DB.Models;
using MSDev.Task.Helpers;
using Newtonsoft.Json.Serialization;
using WebAdmin.FormModels.Catalog;
using AutoMapper;
using MSDev.DB;
using static System.String;

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
			//获取枯级分类
			var topCatalogs = _context.CataLog.Where(m => m.IsTop == 1).ToList();
			ViewBag.TopCatalogs = topCatalogs;
			return View();
		}

		[HttpGet]
		public IActionResult BingNews()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddCatalog(CatalogForm catalogForm)
		{
			if (ModelState.IsValid)
			{
				Catalog topCatalog = null;
				if (IsNullOrEmpty(catalogForm.TopCatalog))
				{
					catalogForm.IsTop = 0;
				}
				else
				{
					catalogForm.IsTop = 1;
					topCatalog = _context.CataLog.Find(Guid.Parse(catalogForm.TopCatalog));
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

				_context.CataLog.Add(catalog);
				_context.SaveChanges();
			}
			return RedirectToAction("Catalog", catalogForm);
		}
	}

}
