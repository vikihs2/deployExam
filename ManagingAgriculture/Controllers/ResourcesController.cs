using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ManagingAgriculture.Models;
using System.Collections.Generic;
using System.Linq;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class ResourcesController : Controller
    {
        /// <summary>
        /// In-memory store for resources (replaces database for prototype).
        /// In production, this would be replaced with EF Core DbContext queries.
        /// </summary>
        private static List<Resource> _resources = new List<Resource>
        {
            new Resource { Id = 1, Name = "NPK Fertilizer 20-20-20", Category = "Fertilizer", Quantity = 150, Unit = "kg", LowStockThreshold = 50, Supplier = "Green Supply Co.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
            new Resource { Id = 2, Name = "Corn Seeds", Category = "Seed", Quantity = 25, Unit = "kg", LowStockThreshold = 30, Supplier = "Seed Masters", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
            new Resource { Id = 3, Name = "Pesticide Spray", Category = "Chemical", Quantity = 80, Unit = "liters", LowStockThreshold = 20, Supplier = "ChemCorp", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
            new Resource { Id = 4, Name = "Irrigation Water", Category = "Water", Quantity = 5000, Unit = "liters", LowStockThreshold = 1000, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
            new Resource { Id = 5, Name = "Diesel Fuel", Category = "Fuel", Quantity = 200, Unit = "liters", LowStockThreshold = 50, Supplier = "FuelCorp", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
        };

        /// <summary>
        /// Display all resources, optionally filtered by category
        /// </summary>
        public IActionResult Index(string? category = null)
        {
            ViewData["Title"] = "Resource Management";
            
            IEnumerable<Resource> resources = _resources;
            
            // Filter by category if provided
            if (!string.IsNullOrEmpty(category) && category != "all")
            {
                resources = resources.Where(r => r.Category.ToLower() == category.ToLower());
            }

            // Group resources by category for the view
            var groupedResources = resources.GroupBy(r => r.Category).ToList();
            
            ViewBag.SelectedCategory = category ?? "all";
            ViewBag.Categories = _resources.Select(r => r.Category).Distinct().ToList();
            
            return View(resources.ToList());
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "Add Resource";
            return View(new Resource());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Resource model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var nextId = _resources.Any() ? _resources.Max(r => r.Id) + 1 : 1;
            model.Id = nextId;
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;
            _resources.Add(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _resources.FirstOrDefault(r => r.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Resource model)
        {
            var existing = _resources.FirstOrDefault(r => r.Id == id);
            if (existing == null) return NotFound();
            if (!ModelState.IsValid) return View(model);

            model.Id = id;
            model.CreatedDate = existing.CreatedDate;
            model.UpdatedDate = DateTime.Now;

            var idx = _resources.FindIndex(r => r.Id == id);
            _resources[idx] = model;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdjustQuantity(int id, int delta)
        {
            var item = _resources.FirstOrDefault(r => r.Id == id);
            if (item != null)
            {
                item.Quantity = Math.Max(0, item.Quantity + delta);
                item.UpdatedDate = DateTime.Now;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// AJAX endpoint to adjust resource quantity and return updated value
        /// </summary>
        [HttpPost]
        public IActionResult AdjustQuantityAjax([FromBody] AdjustRequest req)
        {
            var item = _resources.FirstOrDefault(r => r.Id == req.Id);
            if (item == null)
                return Json(new { success = false });

            item.Quantity = Math.Max(0, item.Quantity + req.Delta);
            item.UpdatedDate = DateTime.Now;

            return Json(new { success = true, quantity = item.Quantity });
        }

        public class AdjustRequest
        {
            public int Id { get; set; }
            public int Delta { get; set; }
        }
    }
}
