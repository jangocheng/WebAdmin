using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;
using MSDev.DB.Entities;

namespace WebAdmin.Controllers
{
    public class PracticeController : BaseController
    {
        private readonly AppDbContext _context;

        public PracticeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Practice
        public async Task<IActionResult> Index()
        {
            return View(await _context.Practice.ToListAsync());
        }

        // GET: Practice/Details/5
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

        // GET: Practice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Practice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CreatedTime,UpdatedTime")] Practice practice)
        {
            if (ModelState.IsValid)
            {
                practice.Id = Guid.NewGuid();
                _context.Add(practice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(practice);
        }

        // GET: Practice/Edit/5
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

        // POST: Practice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Content,CreatedTime,UpdatedTime")] Practice practice)
        {
            if (id != practice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(practice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracticeExists(practice.Id))
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
            return View(practice);
        }

        // GET: Practice/Delete/5
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

        // POST: Practice/Delete/5
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
