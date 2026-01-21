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
        // In a real app we would inject the service, but static for now is fine or we can add to Program.cs
        private readonly ManagingAgriculture.Services.CropDataService _cropService;

        public PlantsController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _cropService = new ManagingAgriculture.Services.CropDataService();
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Plant Tracking";
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            List<Plant> plants;
            if (user.CompanyId != null)
            {
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
            ViewBag.CropCategories = ManagingAgriculture.Services.CropDataService.CropCategories;
            return View(new PlantCreateViewModel { PlantedDate = System.DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PlantCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CropCategories = ManagingAgriculture.Services.CropDataService.CropCategories;
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            
            // Calculate Growth % automatically based on dates if user didn't specify or simplified logic
            int timeGrowth = _cropService.CalculateTimeProgress(model.PlantedDate, model.ExpectedHarvest);
            // We use the calculated time growth, unless user manually overrides (but here we just take the calc for consistency)
            // Or we respect user input if it's not 0? Let's obey the user request: "growth stage % just if you start tracking it after you planted it otherwise its 0%"
            // If PlantedDate is today, it's 0. If PlantedDate was last month, it's X%.
            
            var plant = new Plant
            {
                Name = model.Name,
                PlantType = model.CropType,
                PlantedDate = model.PlantedDate,
                ExpectedHarvestDate = model.ExpectedHarvest,
                GrowthStagePercent = timeGrowth, // Use calculated growth
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
            // RBAC Check: Employee cannot Edit. Manager cannot Edit. Only Boss (or Admin? Assume Boss).
            // Actually, if personal account (no company), user is owner so they can edit.
            // If Company: "manager can add but not edit or delete" + "employee cant add, edit and delete".
            // So only "Boss" can Edit company plants? Or "SystemAdmin"? Let's assume Boss.
            
            var user = await _userManager.GetUserAsync(User);
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null) return NotFound();

            if (user.CompanyId != null)
            {
                // Company Logic
                if (plant.CompanyId != user.CompanyId) return Forbid();
                
                // Role Restrictions
                if (User.IsInRole("Employee") || User.IsInRole("Manager")) 
                {
                    // Manager can ADD, but not EDIT or DELETE.
                    // Employee can only VIEW.
                    return Forbid();
                }
            }
            else
            {
                // Personal Logic - Owner can do whatever
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
            
            ViewBag.CropCategories = ManagingAgriculture.Services.CropDataService.CropCategories;
            ViewBag.Id = plant.Id;
            
            var (score, msg) = _cropService.CalculateGrowthSuitability(plant.PlantType, plant.SoilType, plant.AvgTemperatureCelsius, plant.IsIndoor, plant.WateringFrequencyDays, plant.SunlightExposure);
            ViewBag.AlgorithmScore = score;
            ViewBag.AlgorithmMessage = msg;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlantCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CropCategories = ManagingAgriculture.Services.CropDataService.CropCategories;
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null) return NotFound();

             // Authorization
            if (user.CompanyId != null)
            {
                if (plant.CompanyId != user.CompanyId) return Forbid();
                
                if (User.IsInRole("Employee") || User.IsInRole("Manager")) 
                {
                    return Forbid();
                }
            }
            else
            {
                if (plant.OwnerUserId != user.Id) return Forbid();
            }
            
            int timeGrowth = _cropService.CalculateTimeProgress(model.PlantedDate, model.ExpectedHarvest);

            plant.Name = model.Name;
            plant.PlantType = model.CropType;
            plant.PlantedDate = model.PlantedDate;
            plant.ExpectedHarvestDate = model.ExpectedHarvest;
            plant.GrowthStagePercent = timeGrowth;
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
                    
                     if (User.IsInRole("Employee") || User.IsInRole("Manager")) 
                    {
                        return Forbid();
                    }
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
