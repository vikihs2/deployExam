using Microsoft.AspNetCore.Mvc;
using ManagingAgriculture.Models;
using ManagingAgriculture.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class PlantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public PlantsController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Plant Tracking";
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            List<Plant> plants;
            if (user.CompanyId != null)
            {
                // Show Company plants AND Personal plants (legacy data migration view)
                plants = await _context.Plants
                    .Where(p => p.CompanyId == user.CompanyId || (p.CompanyId == null && p.OwnerUserId == user.Id))
                    .ToListAsync();
            }
            else
            {
                plants = await _context.Plants.Where(p => p.OwnerUserId == user.Id).ToListAsync();
            }
            
            return View(plants);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "Add Plant";
            return View(new PlantCreateViewModel { PlantedDate = System.DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PlantCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            
            var plant = new Plant
            {
                Name = model.Name,
                PlantType = model.CropType,
                PlantedDate = model.PlantedDate,
                ExpectedHarvestDate = model.ExpectedHarvest,
                GrowthStagePercent = model.GrowthStage,
                NextTask = model.NextTask,
                Notes = model.Notes,
                SoilType = model.SoilType,
                SunlightExposure = model.SunlightExposure,
                IsIndoor = model.IsIndoor,
                AvgTemperatureCelsius = model.AvgTemperatureCelsius,
                WateringFrequencyDays = model.WateringFrequencyDays,
                CreatedDate = System.DateTime.UtcNow,
                UpdatedDate = System.DateTime.UtcNow,
                CompanyId = user.CompanyId,
                OwnerUserId = user.CompanyId == null ? user.Id : null
            };

            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null) return NotFound();

            // Authorization Check
            if (user.CompanyId != null)
            {
                if (plant.CompanyId != user.CompanyId) return Forbid();
            }
            else
            {
                if (plant.OwnerUserId != user.Id) return Forbid();
            }

            var model = new PlantCreateViewModel
            {
                Name = plant.Name,
                CropType = plant.PlantType,
                PlantedDate = plant.PlantedDate,
                ExpectedHarvest = plant.ExpectedHarvestDate,
                GrowthStage = plant.GrowthStagePercent,
                NextTask = plant.NextTask,
                Notes = plant.Notes,
                SoilType = plant.SoilType,
                SunlightExposure = plant.SunlightExposure,
                IsIndoor = plant.IsIndoor,
                AvgTemperatureCelsius = plant.AvgTemperatureCelsius,
                WateringFrequencyDays = plant.WateringFrequencyDays
            };

            ViewBag.Id = plant.Id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlantCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null) return NotFound();

             // Authorization Check
            if (user.CompanyId != null)
            {
                if (plant.CompanyId != user.CompanyId) return Forbid();
            }
            else
            {
                if (plant.OwnerUserId != user.Id) return Forbid();
            }

            plant.Name = model.Name;
            plant.PlantType = model.CropType;
            plant.PlantedDate = model.PlantedDate;
            plant.ExpectedHarvestDate = model.ExpectedHarvest;
            plant.GrowthStagePercent = model.GrowthStage;
            plant.NextTask = model.NextTask;
            plant.Notes = model.Notes;
            plant.SoilType = model.SoilType;
            plant.SunlightExposure = model.SunlightExposure;
            plant.IsIndoor = model.IsIndoor;
            plant.AvgTemperatureCelsius = model.AvgTemperatureCelsius;
            plant.WateringFrequencyDays = model.WateringFrequencyDays;
            plant.UpdatedDate = System.DateTime.UtcNow;

            _context.Update(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var plant = await _context.Plants.FindAsync(id);
            if (plant != null)
            {
                 // Authorization Check
                if (user.CompanyId != null)
                {
                    if (plant.CompanyId != user.CompanyId) return Forbid();
                }
                else
                {
                    if (plant.OwnerUserId != user.Id) return Forbid();
                }

                _context.Plants.Remove(plant);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
