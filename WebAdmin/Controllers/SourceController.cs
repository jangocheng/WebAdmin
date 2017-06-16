using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MSDev.DB;
using MSDev.DB.Models;

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
				.Where(m => m.Catalog.Name.Equals("下载"))
				.ToList();

			return View();
		}


		/// <summary>
		/// 添加下载资源
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult AddDownload(Resource resource)
		{
			if (ModelState.IsValid)
			{
				resource.Id = Guid.NewGuid();
				resource.CreatedTime = DateTime.Now;
				resource.UpdatedTime = DateTime.Now;
				_context.Resource.Add(resource);

				_context.SaveChanges();

			}
			return RedirectToAction("Download", resource);
		}
	}

}
