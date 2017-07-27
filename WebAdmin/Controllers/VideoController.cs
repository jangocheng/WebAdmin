using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.FormModels.Resource;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models.ViewModels;
using WebAdmin.Helpers;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public IActionResult Index(int p = 1)
        {
            int pageSize = 12;
            var videos = _context.MvaVideos
                .OrderByDescending(m => m.UpdatedTime)
                .Skip((p - 1) * pageSize).Take(pageSize)
                .ToList();

            int totalNumber = _context.MvaVideos.Count();

            ViewBag.ListData = videos;

            var pageOption = new MyPagerOption()
            {
                CurrentPage = p,
                PageSize = pageSize,
                RouteUrl = "/Video/Index",
                Total = totalNumber
            };
            ViewBag.Pager = pageOption;

            return View(videos);
        }
            
        [HttpPost]
        public IActionResult DelMvaVideo(string id)
        {
            MvaVideos video = _context.MvaVideos.Find(Guid.Parse(id));
            _context.MvaVideos.Remove(video);
            var re = _context.SaveChanges();
            if (re > 0)
            {
                return JsonOk(re);
            }
            return JsonFailed();
        }
        [HttpGet]
        public IActionResult RecommendMva(string id)
        {
            var video = _context.MvaVideos.Find(Guid.Parse(id));
            video.IsRecommend = true;
            _context.MvaVideos.Update(video);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
