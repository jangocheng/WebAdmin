using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;
using MSDev.DB.Entities;
using Newtonsoft.Json;
using WebAdmin.Helpers;

namespace WebAdmin.Controllers
{
    public class CommodityController : Controller
    {
        private readonly AppDbContext _context;

        public CommodityController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Commodity.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commodity = await _context.Commodity
                .SingleOrDefaultAsync(m => m.Id == id);
            if (commodity == null)
            {
                return NotFound();
            }

            return View(commodity);
        }

        public IActionResult Create()
        {
            // 查询关联服务(视频)
            ViewBag.Services = _context.Catalog
                .Where(m => m.IsTop == 0 && m.Type.Equals("视频"))
                .Where(m => m.TopCatalog.Value.Equals("CourseVideo"))
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SerialNumber,TargetId,Type,Name,OriginPrice,Price,CurrentNumber,Description,Thumbnail")] Commodity commodity)
        {
            Console.WriteLine(JsonConvert.SerializeObject(commodity));
            if (ModelState.IsValid)
            {
                _context.Add(commodity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commodity);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commodity = await _context.Commodity.SingleOrDefaultAsync(m => m.Id == id);
            if (commodity == null)
            {
                return NotFound();
            }
            // 查询关联服务(视频)
            ViewBag.Services = _context.Catalog
                .Where(m => m.IsTop == 0 && m.Type.Equals("视频"))
                .Where(m => m.TopCatalog.Value.Equals("CourseVideo"))
                .ToList();

            return View(commodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SerialNumber,Name,OriginPrice,Price,CurrentNumber,Description,Thumbnail,CategoryId,Id,CreatedTime")] Commodity commodity)
        {
            if (id != commodity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    commodity.Status = StatusType.Edit;
                    _context.Update(commodity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommodityExists(commodity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(commodity);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commodity = await _context.Commodity
                .SingleOrDefaultAsync(m => m.Id == id);
            if (commodity == null)
            {
                return NotFound();
            }

            return View(commodity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var commodity = await _context.Commodity.SingleOrDefaultAsync(m => m.Id == id);
            _context.Commodity.Remove(commodity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommodityExists(Guid id)
        {
            return _context.Commodity.Any(e => e.Id == id);
        }
    }
}
