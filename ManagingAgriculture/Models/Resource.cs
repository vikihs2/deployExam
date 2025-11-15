using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
	public class Resource
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[StringLength(50)]
		public string Category { get; set; } = string.Empty;

		[Column(TypeName = "decimal(10,2)")]
		public decimal Quantity { get; set; }

		[StringLength(20)]
		public string Unit { get; set; } = string.Empty;

		[Column(TypeName = "decimal(10,2)")]
		public decimal LowStockThreshold { get; set; }

		[StringLength(100)]
		public string? Supplier { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }

		// Navigation
		public ICollection<ResourceUsage>? ResourceUsages { get; set; }
	}
}
