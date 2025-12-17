using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ManagingAgriculture.Models;

namespace ManagingAgriculture.Controllers
{
    /// <summary>
    /// MachineryController manages farm equipment inventory and maintenance tracking.
    /// Handles viewing, adding, editing, and deleting machinery records with maintenance history.
    /// </summary>
    [Authorize]
    public class MachineryController : Controller
    {
        /// <summary>
        /// In-memory data store for machinery collection.
        /// This is a prototype implementation using a static List.
        /// In production, this would be replaced with Entity Framework Core database access.
        /// </summary>
        private static List<Machinery> _machineryList = new()
        {
            new Machinery
            {
                Id = 1,
                Name = "John Deere 5075E",
                Type = "Tractor",
                Status = "Excellent",
                PurchaseDate = new DateTime(2022, 3, 15),
                LastServiceDate = new DateTime(2025, 11, 5),
                NextServiceDate = new DateTime(2025, 12, 5),
                PurchasePrice = 45000,
                CreatedDate = new DateTime(2022, 3, 15),
                UpdatedDate = new DateTime(2025, 11, 5)
            },
            new Machinery
            {
                Id = 2,
                Name = "Kubota M5-091",
                Type = "Tractor",
                Status = "Good",
                PurchaseDate = new DateTime(2021, 6, 20),
                LastServiceDate = new DateTime(2025, 10, 15),
                NextServiceDate = new DateTime(2025, 11, 15),
                PurchasePrice = 38000,
                CreatedDate = new DateTime(2021, 6, 20),
                UpdatedDate = new DateTime(2025, 10, 15)
            },
            new Machinery
            {
                Id = 3,
                Name = "Claas Lexion 780",
                Type = "Combine Harvester",
                Status = "Fair",
                PurchaseDate = new DateTime(2019, 8, 10),
                LastServiceDate = new DateTime(2025, 9, 1),
                NextServiceDate = new DateTime(2025, 12, 1),
                PurchasePrice = 85000,
                CreatedDate = new DateTime(2019, 8, 10),
                UpdatedDate = new DateTime(2025, 9, 1)
            }
        };

        /// <summary>
        /// Displays list of all machinery in the farm inventory.
        /// Returns machinery sorted by status (Excellent first).
        /// </summary>
        /// <returns>View with IEnumerable of Machinery</returns>
        public IActionResult Index()
        {
            ViewData["Title"] = "Machinery Management";

            // Sort machinery by status priority for better visibility
            var machinery = _machineryList.OrderBy(m => m.Status switch
            {
                "Excellent" => 1,
                "Good" => 2,
                "Fair" => 3,
                "Poor" => 4,
                _ => 5
            }).ToList();

            return View(machinery);
        }

        // Expose the in-memory list so other controllers can reference user machinery
        public static List<Machinery> GetAll() => _machineryList;

        [HttpGet]
        public IActionResult DetailsApi(int id)
        {
            var mach = _machineryList.FirstOrDefault(m => m.Id == id);
            if (mach == null) return NotFound();
            return Json(new { id = mach.Id, name = mach.Name, type = mach.Type, status = mach.Status });
        }

        /// <summary>
        /// Displays form to add new machinery.
        /// HTTP GET method for displaying empty form.
        /// </summary>
        /// <returns>View with empty Machinery model</returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View(new Machinery());
        }

        /// <summary>
        /// Processes form submission to create new machinery record.
        /// HTTP POST method that validates input and adds machinery to collection.
        /// </summary>
        /// <param name="machinery">Machinery object populated from form submission</param>
        /// <returns>Redirects to Index on success, returns view with model on validation failure</returns>
        [HttpPost]
        public IActionResult Add(Machinery machinery)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return View(machinery);
            }

            // Additional validation: Engine hours must not be negative
            if (machinery.EngineHours.HasValue && machinery.EngineHours.Value < 0)
            {
                ModelState.AddModelError(nameof(machinery.EngineHours), "Engine hours cannot be negative");
                return View(machinery);
            }

            // Assign new ID (max ID + 1)
            machinery.Id = _machineryList.Count > 0 ? _machineryList.Max(m => m.Id) + 1 : 1;

            // Set timestamps for audit trail
            machinery.CreatedDate = DateTime.Now;
            machinery.UpdatedDate = DateTime.Now;

            // Add to collection
            _machineryList.Add(machinery);

            // Redirect to machinery list view
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays machinery details and edit form.
        /// HTTP GET method for displaying machinery information by ID.
        /// </summary>
        /// <param name="id">ID of machinery to display</param>
        /// <returns>View with Machinery model if found, or NotFound if machinery doesn't exist</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Find machinery by ID
            var machinery = _machineryList.FirstOrDefault(m => m.Id == id);

            if (machinery == null)
            {
                return NotFound();
            }

            return View(machinery);
        }

        /// <summary>
        /// Processes form submission to update existing machinery record.
        /// HTTP POST method that validates input and updates machinery in collection.
        /// </summary>
        /// <param name="id">ID of machinery to update</param>
        /// <param name="machinery">Updated Machinery object from form submission</param>
        /// <returns>Redirects to Index on success, returns view with model on validation failure</returns>
        [HttpPost]
        public IActionResult Edit(int id, Machinery machinery)
        {
            // Find machinery in collection
            var existingMachinery = _machineryList.FirstOrDefault(m => m.Id == id);

            if (existingMachinery == null)
            {
                return NotFound();
            }

            // Validate model state
            if (!ModelState.IsValid)
            {
                return View(machinery);
            }

            // Additional validation: Engine hours must not be negative
            if (machinery.EngineHours.HasValue && machinery.EngineHours.Value < 0)
            {
                ModelState.AddModelError(nameof(machinery.EngineHours), "Engine hours cannot be negative");
                return View(machinery);
            }

            // Preserve creation date and ID
            machinery.Id = id;
            machinery.CreatedDate = existingMachinery.CreatedDate;
            machinery.UpdatedDate = DateTime.Now;

            // Update record in collection
            var index = _machineryList.FindIndex(m => m.Id == id);
            _machineryList[index] = machinery;

            // Redirect to machinery list view
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Deletes machinery record from inventory.
        /// Removes the machinery with specified ID from collection.
        /// </summary>
        /// <param name="id">ID of machinery to delete</param>
        /// <returns>Redirects to Index after deletion</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Find and remove machinery from collection
            var machinery = _machineryList.FirstOrDefault(m => m.Id == id);

            if (machinery != null)
            {
                _machineryList.Remove(machinery);
            }

            // Redirect to machinery list view
            return RedirectToAction(nameof(Index));
        }
    }
}
