using Microsoft.AspNetCore.Mvc;
using ManagingAgriculture.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class PlantsController : Controller
    {
        // Temporary in-memory store for plants. In real app replace with DbContext.
        private static readonly List<Plant> _plants = new();

        static PlantsController()
        {
            // seed with some example plants if empty
            if (!_plants.Any())
            {
                _plants.Add(new Plant
                {
                    Id = 1,
                    Name = "Tomato Batch A",
                    PlantType = "Tomato",
                    PlantedDate = new System.DateTime(2025, 3, 1),
                    ExpectedHarvestDate = new System.DateTime(2025, 6, 15),
                    GrowthStagePercent = 40,
                    NextTask = "Water in 2 days",
                    Notes = "Greenhouse 1",
                    CreatedDate = System.DateTime.UtcNow,
                    UpdatedDate = System.DateTime.UtcNow
                });
                _plants.Add(new Plant
                {
                    Id = 2,
                    Name = "Corn Field B",
                    PlantType = "Corn",
                    PlantedDate = new System.DateTime(2025, 4, 10),
                    ExpectedHarvestDate = new System.DateTime(2025, 9, 5),
                    GrowthStagePercent = 10,
                    NextTask = "Fertilize soon",
                    Notes = "Needs monitoring",
                    CreatedDate = System.DateTime.UtcNow,
                    UpdatedDate = System.DateTime.UtcNow
                });
            }
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Plant Tracking";
            return View(_plants);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "Add Plant";
            return View(new PlantCreateViewModel { PlantedDate = System.DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(PlantCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var nextId = _plants.Any() ? _plants.Max(p => p.Id) + 1 : 1;
            var plant = new Plant
            {
                Id = nextId,
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

            _plants.Add(plant);

            return RedirectToAction("Index");
        }
    }
}
