using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ManagingAgriculture.Models;
using ManagingAgriculture.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class ResourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Display all resources, optionally filtered by category
        /// </summary>
        public async Task<IActionResult> Index(string? category = null)
        {
            ViewData["Title"] = "Resource Management";
            
            var query = _context.Resources.AsQueryable();
            
            // Filter by category if provided
            if (!string.IsNullOrEmpty(category) && category != "all")
            {
                query = query.Where(r => r.Category == category);
            }

            var resources = await query.ToListAsync();
            
            // Get all categories for the filter dropdown
            ViewBag.Categories = await _context.Resources.Select(r => r.Category).Distinct().ToListAsync();
            ViewBag.SelectedCategory = category ?? "all";
            
            return View(resources);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "Add Resource";
            return View(new Resource());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Resource model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;
            
            _context.Resources.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Resources.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Resource model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            try
            {
                var existing = await _context.Resources.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                if (existing == null) return NotFound();

                model.CreatedDate = existing.CreatedDate;
                model.UpdatedDate = DateTime.Now;

                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(id)) return NotFound();
                else throw;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdjustQuantity(int id, int delta)
        {
            var item = await _context.Resources.FindAsync(id);
            if (item != null)
            {
                item.Quantity = Math.Max(0, item.Quantity + delta);
                item.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// AJAX endpoint to adjust resource quantity and return updated value
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AdjustQuantityAjax([FromBody] AdjustRequest req)
        {
            var item = await _context.Resources.FindAsync(req.Id);
            if (item == null)
                return Json(new { success = false });

            item.Quantity = Math.Max(0, item.Quantity + req.Delta);
            item.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true, quantity = item.Quantity });
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }

        public class AdjustRequest
        {
            public int Id { get; set; }
            public int Delta { get; set; }
        }
    }
}
