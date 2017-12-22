using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;
using MSDev.DB.Entities;
using WebAdmin.Helpers;

namespace WebAdmin.Controllers
{
    public class PracticeController : BaseController
    {
        private readonly AppDbContext _context;

        public PracticeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int p = 1, string series = "")
        {
            int pageSize = 12;
            var practices = _context.Practice.OrderByDescending(m => m.CreatedTime)
                .Where(m => m.Catalog.Value.Equals(series) || series == "")
                .Include(m => m.Catalog)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalNumber = _context.Practice.Count();

            ViewBag.Catalogs = _context.Catalog
                .Where(m => m.Type.Equals("实践"))
                .ToList();

            SetPage(p, pageSize, totalNumber);
            return View(practices);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practice = await _context.Practice
                .SingleOrDefaultAsync(m => m.Id == id);
            if (practice == null)
            {
                return NotFound();
            }

            return View(practice);
        }

        public IActionResult Create()
        {
            //查询分类
            var catalogs = _context.Catalog
             .Where(m => m.TopCatalog.Value.Equals("PracticeCourse"))
             .ToList();

            ViewBag.Catalogs = catalogs;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Practice practice, Guid CatalogId)
        {
            if (ModelState.IsValid)
            {
                practice.Id = Guid.NewGuid();
                practice.CreatedTime = DateTime.Now;
                practice.UpdatedTime = DateTime.Now;
                practice.Status = StatusType.New;
                practice.Views = 0;
                practice.Catalog = _context.Catalog.Find(CatalogId);

                _context.Add(practice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(practice);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practice = await _context.Practice.SingleOrDefaultAsync(m => m.Id == id);
            if (practice == null)
            {
                return NotFound();
            }
            return View(practice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Content")] Practice practice)
        {
            if (id != practice.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var currentPractice = _context.Practice.Find(id);
                    if (currentPractice == null)
                    {
                        return NotFound();
                    }
                    currentPractice.Title = practice.Title;
                    currentPractice.Content = practice.Content;
                    currentPractice.UpdatedTime = DateTime.Now;
                    currentPractice.Status = StatusType.Edit;
                    _context.Update(currentPractice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //TODO:异常处理
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(practice);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practice = await _context.Practice
                .SingleOrDefaultAsync(m => m.Id == id);
            if (practice == null)
            {
                return NotFound();
            }

            return View(practice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var practice = await _context.Practice.SingleOrDefaultAsync(m => m.Id == id);
            _context.Practice.Remove(practice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PracticeExists(Guid id)
        {
            return _context.Practice.Any(e => e.Id == id);
        }
    }
}
