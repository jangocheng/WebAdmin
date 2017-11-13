using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.FormModels.Resource;

namespace WebAdmin.Controllers
{
    /// <summary>
    /// 资源管理,如下载资源
    /// </summary>
    public class ResourceController : BaseController
    {
        private readonly AppDbContext _context;
        public ResourceController( AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region 下载相关
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
                .SingleOrDefault(m => m.Id == Guid.Parse(id));
            return View(resource);
        }

        [HttpPost]
        public IActionResult UpdateDownload(Resource resource)
        {
            if (ModelState.IsValid)
            {
                if (_context.Resource.Any(m => m.Id == resource.Id))
                {
                    resource.UpdatedTime = DateTime.Now;
                    _context.Resource.Update(resource);
                    if (_context.SaveChanges() > 0)
                    {
                        return RedirectToAction("Download");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "更新失败");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "不存在该元素");
                }
            }
            return View(resource);
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
                    Provider = resourceForm.Provider,
                    Tag = resourceForm.Provider,
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
        #endregion

        #region 文档相关
        /// <summary>
        /// 文档管理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Document()
        {
            ViewBag.Documents = _context.Resource
                .Where(m => m.Catalog.Type.Equals("文档"))
                .ToList();

            ViewBag.Catalogs = _context.Catalog.Where(m => m.Type == "文档").ToList();

            return View();
        }

        [HttpGet]
        public IActionResult EditDocument(string id)
        {
            ViewBag.Downloads = _context.Resource
                .Where(m => m.Catalog.Type.Equals("文档"))
                .ToList();

            var resource = _context.Resource
                .SingleOrDefault(m => m.Id == Guid.Parse(id));
            return View(resource);
        }

        [HttpPost]
        public IActionResult UpdateDocument(Resource resource)
        {
            if (ModelState.IsValid)
            {
                if (_context.Resource.Any(m => m.Id == resource.Id))
                {
                    resource.UpdatedTime = DateTime.Now;
                    _context.Resource.Update(resource);
                    if (_context.SaveChanges() > 0)
                    {
                        return RedirectToAction("Document");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "更新失败");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "不存在该元素");
                }
            }
            return View(resource);
        }


        /// <summary>
        /// 添加文档资源
        /// </summary>
        /// <param name="resourceForm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddDocument(ResourceForm resourceForm)
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
                    Provider = resourceForm.Provider,
                    Tag = resourceForm.Provider,
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
                return RedirectToAction("Document");
            }

            return JumpPage();
        }

        /// <summary>
        /// 删除文档资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DelDocument([FromRoute]string id)
        {
            var resource = _context.Resource.Find(Guid.Parse(id));
            if (resource != null)
            {
                _context.Resource.Remove(resource);
                _context.SaveChangesAsync();
            }

            return RedirectToAction("Document");
        }
        #endregion

        #region 项目相关
        /// <summary>
        /// 项目管理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Project()
        {
            ViewBag.Projects = _context.Resource
                .Where(m => m.Catalog.Type.Equals("项目"))
                .ToList();

            ViewBag.Catalogs = _context.Catalog.Where(m => m.Type == "项目").ToList();

            return View();
        }

        [HttpGet]
        public IActionResult EditProject(string id)
        {
            ViewBag.Projects = _context.Resource
                .Where(m => m.Catalog.Type.Equals("项目"))
                .ToList();

            var resource = _context.Resource
                .SingleOrDefault(m => m.Id == Guid.Parse(id));
            return View(resource);
        }

        [HttpPost]
        public IActionResult UpdateProject(Resource resource)
        {
            if (ModelState.IsValid)
            {
                if (_context.Resource.Any(m => m.Id == resource.Id))
                {
                    resource.UpdatedTime = DateTime.Now;
                    _context.Resource.Update(resource);
                    if (_context.SaveChanges() > 0)
                    {
                        return RedirectToAction("Project");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "更新失败");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "不存在该元素");
                }
            }
            return View(resource);
        }


        /// <summary>
        /// 添加项目资源
        /// </summary>
        /// <param name="resourceForm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddProject(ResourceForm resourceForm)
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
                    Provider = resourceForm.Provider,
                    Tag = resourceForm.Provider,
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
                return RedirectToAction("Project");
            }

            return JumpPage();
        }

        /// <summary>
        /// 删除下载资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DelProject([FromRoute]string id)
        {
            var resource = _context.Resource.Find(Guid.Parse(id));
            if (resource != null)
            {
                _context.Resource.Remove(resource);
                _context.SaveChangesAsync();
            }

            return RedirectToAction("Project");
        }
        #endregion
    }

}
