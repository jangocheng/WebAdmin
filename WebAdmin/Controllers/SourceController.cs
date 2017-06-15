using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MSDev.DB;

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
			return View();
		}
	}

}
