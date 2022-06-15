using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LegionWebApp.Data;
using LegionWebApp.Models;

namespace LegionWebApp.Controllers
{
    public class ContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Content
        public async Task<IActionResult> Index()
        {
              return _context.ContentModel != null ? 
                          View(await _context.ContentModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ContentModel'  is null.");
        }

        // GET: Content/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContentModel == null)
            {
                return NotFound();
            }

            var contentModel = await _context.ContentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contentModel == null)
            {
                return NotFound();
            }

            return View(contentModel);
        }

        // GET: Content/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Content/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Context,DatePublished,UserPublished")] ContentModel contentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contentModel);
        }

        // GET: Content/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContentModel == null)
            {
                return NotFound();
            }

            var contentModel = await _context.ContentModel.FindAsync(id);
            if (contentModel == null)
            {
                return NotFound();
            }
            return View(contentModel);
        }

        // POST: Content/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Context,DatePublished,UserPublished")] ContentModel contentModel)
        {
            if (id != contentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentModelExists(contentModel.Id))
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
            return View(contentModel);
        }

        // GET: Content/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContentModel == null)
            {
                return NotFound();
            }

            var contentModel = await _context.ContentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contentModel == null)
            {
                return NotFound();
            }

            return View(contentModel);
        }

        // POST: Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContentModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ContentModel'  is null.");
            }
            var contentModel = await _context.ContentModel.FindAsync(id);
            if (contentModel != null)
            {
                _context.ContentModel.Remove(contentModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentModelExists(int id)
        {
          return (_context.ContentModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
