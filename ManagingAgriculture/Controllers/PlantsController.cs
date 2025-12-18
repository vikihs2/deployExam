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

        public PlantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Plant Tracking";
            var plants = await _context.Plants.ToListAsync();
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
                UpdatedDate = System.DateTime.UtcNow
            };

            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
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

            ViewBag.Id = plant.Id; // Pass ID to view for form submission
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlantCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
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
            var plant = await _context.Plants.FindAsync(id);
            if (plant != null)
            {
                _context.Plants.Remove(plant);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
