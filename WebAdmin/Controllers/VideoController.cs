using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.Helpers;
using WebAdmin.Models.ViewModels;

namespace WebAdmin.Controllers
{
    /// <summary>
    /// 资源管理,如下载资源
    /// </summary>
    public class VideoController : BaseController
    {
        private readonly AppDbContext _context;
        public VideoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int p = 1)
        {
            int pageSize = 12;
            var videos = _context.Video
                .OrderByDescending(m => m.UpdatedTime)
                .Skip((p - 1) * pageSize).Take(pageSize)
                .ToList();

            int totalNumber = _context.Video.Count();

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

        [HttpGet]
        public IActionResult MvaVideo(int p = 1)
        {
            int pageSize = 12;
            var videos = _context.MvaVideos
                .Where(m => m.LanguageCode.Equals("zh-cn"))
                .OrderByDescending(m => m.UpdatedTime)
                .Skip((p - 1) * pageSize).Take(pageSize)
                .ToList();

            int totalNumber = _context.MvaVideos.Count();

            ViewBag.ListData = videos;

            var pageOption = new MyPagerOption()
            {
                CurrentPage = p,
                PageSize = pageSize,
                RouteUrl = "/Video/MvaVideo",
                Total = totalNumber
            };
            ViewBag.Pager = pageOption;

            return View(videos);
        }

        [HttpGet]
        public IActionResult AddVideo()
        {
            var catalogs = _context.Catalog
                .Where(m => m.TopCatalog.Value.Equals("CourseVideo"))
                .ToList();

            ViewBag.Catalogs = catalogs;
            return View();
        }

        [HttpPost]
        public IActionResult AddVideo(Video video)
        {
            if (ModelState.IsValid)
            {
                video.Id = Guid.NewGuid();
                video.Status = "new";
                video.IsRecommend = false;
                video.Views = 0;
                video.CreatedTime = DateTime.Now;
                video.UpdatedTime = DateTime.Now;

                _context.Video.Add(video);
                var re = _context.SaveChanges();
                if (re > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditVideo(string id)
        {
            var video = _context.Video.Find(Guid.Parse(id));

            if (video == null) { return NotFound(); }
            var catalogs = _context.Catalog
             .Where(m => m.TopCatalog.Value.Equals("CourseVideo"))
             .ToList();

            ViewBag.Catalogs = catalogs;
            return View(video);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditVideo(Video videoForm)
        {
            if (ModelState.IsValid)
            {
                var oldVideo = _context.Video.Find(videoForm.Id);
                if (oldVideo == null)
                {
                    return NotFound();
                }
                videoForm.UpdatedTime = DateTime.Now;
                _context.Entry(oldVideo).CurrentValues.SetValues(videoForm);
                var re = _context.SaveChanges();
                if (re > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(EditVideo));
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

        [HttpPost]
        public IActionResult DelVideo(string id)
        {
            var video = _context.Video.Find(Guid.Parse(id));
            _context.Video.Remove(video);
            var re = _context.SaveChanges();
            if (re > 0)
            {
                return JsonOk(re);
            }
            return JsonFailed();
        }

        [HttpGet]
        public IActionResult EditMvaVideo(string id)
        {
            var video = _context.MvaVideos.Find(Guid.Parse(id));
            return View(video);
        }

        [HttpPost]
        public IActionResult EditMvaVideo(MvaVideos mvaVideo)
        {

            if (ModelState.IsValid)
            {
                var video = _context.MvaVideos.Find(mvaVideo.Id);
                video.Tags = mvaVideo.Tags;
                video.Technologies = mvaVideo.Technologies;
                video.IsRecommend = mvaVideo.IsRecommend;

                _context.Update(video);
                var re = _context.SaveChanges();
                if (re > 0)
                {
                    return View(video);
                }
            }

            return View(mvaVideo);
        }
    }

}
