using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.FormModels.Resource;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models.ViewModels;

namespace WebAdmin.Controllers
{
    /// <summary>
    /// 资源管理,如下载资源
    /// </summary>
    public class VideoController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public VideoController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }

}
