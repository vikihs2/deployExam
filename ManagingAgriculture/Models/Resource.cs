using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
	/// <summary>
	/// Represents a farm resource/supply item (fertilizer, seeds, chemicals, fuel, water, etc.).
	/// Tracks inventory quantities, categories, suppliers, and low-stock thresholds.
	/// </summary>
	public class Resource
	{
		/// <summary>Primary key - unique identifier</summary>
		[Key]
		public int Id { get; set; }

		/// <summary>Name of the resource (e.g., 'NPK Fertilizer 20-20-20', 'Corn Seeds')</summary>
		[Required]
		[StringLength(100)]
		public string Name { get; set; } = string.Empty;

		/// <summary>Category: Fertilizer, Seed, Chemical, Water, Fuel, etc.</summary>
		[Required]
		[StringLength(50)]
		public string Category { get; set; } = string.Empty;

		/// <summary>Current quantity in inventory</summary>
		[Column(TypeName = "decimal(10,2)")]
		public decimal Quantity { get; set; }

		/// <summary>Unit of measurement (kg, liters, units, etc.)</summary>
		[StringLength(20)]
		public string Unit { get; set; } = string.Empty;

		/// <summary>Threshold quantity - alert when below this value</summary>
		[Column(TypeName = "decimal(10,2)")]
		public decimal LowStockThreshold { get; set; }

		/// <summary>Supplier/vendor name</summary>
		[StringLength(100)]
		public string? Supplier { get; set; }

		/// <summary>Record creation date</summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>Last update date</summary>
		public DateTime UpdatedDate { get; set; }

		// ===== NAVIGATION PROPERTIES =====

		/// <summary>Resource usage records tracking consumption of this resource</summary>
		public ICollection<ResourceUsage>? ResourceUsages { get; set; }
	}
}
