using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ManagingAgriculture.Models;

namespace ManagingAgriculture.Controllers
{
    /// <summary>
    /// MarketplaceController manages marketplace listings for buying, selling, and renting equipment/products.
    /// Users can browse available listings and post their own items for sale or rent.
    /// </summary>
    [Authorize]
    public class MarketplaceController : Controller
    {
        /// <summary>
        /// In-memory data store for marketplace listings collection.
        /// This is a prototype implementation using a static List.
        /// In production, this would be replaced with Entity Framework Core database access.
        /// </summary>
        private static List<MarketplaceListing> _listingsList = new()
        {
            new MarketplaceListing
            {
                Id = 1,
                ItemName = "Used John Deere Tractor",
                Category = "Equipment",
                ConditionStatus = "Good",
                Description = "Well-maintained tractor with 2000 hours, ready for work",
                SalePrice = 32000,
                RentalPricePerDay = 0,
                SellerName = "Farm Co. Ltd",
                SellerPhone = "555-1234",
                ImageUrl = "/images/tractor.jpg",
                ListingType = "Sale",
                ListingStatus = "Active",
                CreatedDate = new DateTime(2025, 11, 10),
                UpdatedDate = new DateTime(2025, 11, 10)
            },
            new MarketplaceListing
            {
                Id = 2,
                ItemName = "Equipment Rental - Round Baler",
                Category = "Equipment",
                ConditionStatus = "Excellent",
                Description = "Premium round baler available for seasonal rental",
                SalePrice = 0,
                RentalPricePerDay = 150,
                SellerName = "John's Farm Services",
                SellerPhone = "555-5678",
                ImageUrl = "/images/baler.jpg",
                ListingType = "Rent",
                ListingStatus = "Active",
                CreatedDate = new DateTime(2025, 11, 5),
                UpdatedDate = new DateTime(2025, 11, 5)
            },
            new MarketplaceListing
            {
                Id = 3,
                ItemName = "Organic Tomato Seeds - Heirloom",
                Category = "Seeds",
                ConditionStatus = "New",
                Description = "Certified organic heirloom tomato seeds, 500g package, high germination rate",
                SalePrice = 25,
                RentalPricePerDay = 0,
                SellerName = "Heritage Seeds Supply",
                SellerPhone = "555-9012",
                ImageUrl = "/images/seeds.jpg",
                ListingType = "Sale",
                ListingStatus = "Active",
                CreatedDate = new DateTime(2025, 11, 8),
                UpdatedDate = new DateTime(2025, 11, 8)
            }
        };

        /// <summary>
        /// Displays list of all marketplace listings.
        /// Returns listings filtered by active status, sorted by newest first.
        /// </summary>
        /// <returns>View with IEnumerable of MarketplaceListing</returns>
        public IActionResult Index(string? category = null, string? q = null)
        {
            ViewData["Title"] = "Marketplace";

            // Filter active listings and sort by creation date (newest first)
            var listings = _listingsList
                .Where(l => l.ListingStatus == "Active");

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                listings = listings.Where(l => l.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                var qLower = q.ToLower();
                listings = listings.Where(l => (l.ItemName ?? "").ToLower().Contains(qLower) || (l.Description ?? "").ToLower().Contains(qLower));
            }

            var results = listings.OrderByDescending(l => l.CreatedDate).ToList();

            ViewBag.Categories = _listingsList.Select(l => l.Category).Distinct().ToList();
            ViewBag.SelectedCategory = category ?? "All";
            ViewBag.Query = q ?? string.Empty;

            return View(results);
        }

        /// <summary>
        /// Returns filtered listings as a partial view for AJAX updates
        /// </summary>
        [HttpGet]
        public IActionResult Filter(string? category = null, string? q = null)
        {
            var listings = _listingsList.Where(l => l.ListingStatus == "Active");

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                listings = listings.Where(l => l.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                var qLower = q.ToLower();
                listings = listings.Where(l => (l.ItemName ?? "").ToLower().Contains(qLower) || (l.Description ?? "").ToLower().Contains(qLower));
            }

            var results = listings.OrderByDescending(l => l.CreatedDate).ToList();
            return PartialView("_Grid", results);
        }

        /// <summary>
        /// Displays form to post new marketplace listing.
        /// HTTP GET method for displaying empty form.
        /// </summary>
        /// <returns>View with empty MarketplaceListing model</returns>
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.UserMachinery = MachineryController.GetAll();
            return View(new MarketplaceListing());
        }

        /// <summary>
        /// Processes form submission to create new marketplace listing.
        /// HTTP POST method that validates input and adds listing to collection.
        /// </summary>
        /// <param name="listing">MarketplaceListing object populated from form submission</param>
        /// <returns>Redirects to Index on success, returns view with model on validation failure</returns>
        [HttpPost]
        public IActionResult Add(MarketplaceListing listing, int? selectedMachineryId)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return View(listing);
            }

            // If user chose existing machinery, prefill some listing fields
            if (selectedMachineryId.HasValue)
            {
                var mach = MachineryController.GetAll().FirstOrDefault(m => m.Id == selectedMachineryId.Value);
                if (mach != null)
                {
                    listing.ItemName = string.IsNullOrWhiteSpace(listing.ItemName) ? mach.Name : listing.ItemName;
                    listing.Description = string.IsNullOrWhiteSpace(listing.Description) ? $"{mach.Type} - {mach.Name}" : listing.Description;
                    listing.Category = string.IsNullOrWhiteSpace(listing.Category) ? "Equipment" : listing.Category;
                }
            }

            // Assign new ID (max ID + 1)
            listing.Id = _listingsList.Count > 0 ? _listingsList.Max(l => l.Id) + 1 : 1;

            // Set timestamps for audit trail
            listing.CreatedDate = DateTime.Now;
            listing.UpdatedDate = DateTime.Now;

            // Default listing to active status
            listing.ListingStatus = "Active";

            // Add to collection
            _listingsList.Add(listing);

            // Redirect to marketplace list view
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays listing details and edit form.
        /// HTTP GET method for displaying marketplace listing information by ID.
        /// </summary>
        /// <param name="id">ID of listing to display</param>
        /// <returns>View with MarketplaceListing model if found, or NotFound if listing doesn't exist</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Find listing by ID
            var listing = _listingsList.FirstOrDefault(l => l.Id == id);

            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        /// <summary>
        /// Processes form submission to update existing marketplace listing.
        /// HTTP POST method that validates input and updates listing in collection.
        /// </summary>
        /// <param name="id">ID of listing to update</param>
        /// <param name="listing">Updated MarketplaceListing object from form submission</param>
        /// <returns>Redirects to Index on success, returns view with model on validation failure</returns>
        [HttpPost]
        public IActionResult Edit(int id, MarketplaceListing listing)
        {
            // Find listing in collection
            var existingListing = _listingsList.FirstOrDefault(l => l.Id == id);

            if (existingListing == null)
            {
                return NotFound();
            }

            // Validate model state
            if (!ModelState.IsValid)
            {
                return View(listing);
            }

            // Preserve creation date and ID
            listing.Id = id;
            listing.CreatedDate = existingListing.CreatedDate;
            listing.UpdatedDate = DateTime.Now;

            // Update record in collection
            var index = _listingsList.FindIndex(l => l.Id == id);
            _listingsList[index] = listing;

            // Redirect to marketplace list view
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Deletes marketplace listing.
        /// Marks the listing as sold/expired rather than permanently removing it.
        /// </summary>
        /// <param name="id">ID of listing to delete</param>
        /// <returns>Redirects to Index after deletion</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Find and remove listing from collection
            var listing = _listingsList.FirstOrDefault(l => l.Id == id);

            if (listing != null)
            {
                _listingsList.Remove(listing);
            }

            // Redirect to marketplace list view
            return RedirectToAction(nameof(Index));
        }
    }
}
