using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ManagingAgriculture.Data.ApplicationDbContext _context;

        public DashboardController(ManagingAgriculture.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard";

            var viewModel = new ManagingAgriculture.ViewModels.DashboardViewModel
            {
                ActivePlantsCount = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(_context.Plants.Where(p => p.Status != "Harvested")),
                ResourcesCount = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(_context.Resources),
                MachineryCount = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(_context.Machinery),
                LowStockCount = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(_context.Resources.Where(r => r.Quantity < r.LowStockThreshold)),
                ActiveCrops = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(_context.Plants
                    .Where(p => p.Status != "Harvested")
                    .OrderByDescending(p => p.CreatedDate)
                    .Take(5))
            };

            return View(viewModel);
        }
    }
}
