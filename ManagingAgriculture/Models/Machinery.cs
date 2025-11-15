using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
	public class Machinery
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[StringLength(50)]
		public string Type { get; set; } = string.Empty;

		[Column(TypeName = "date")]
		public DateTime? PurchaseDate { get; set; }

		[StringLength(20)]
		public string Status { get; set; } = string.Empty;

		[Column(TypeName = "decimal(10,2)")]
		public decimal? PurchasePrice { get; set; }

		[Column(TypeName = "date")]
		public DateTime? LastServiceDate { get; set; }

		[Column(TypeName = "date")]
		public DateTime? NextServiceDate { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }

		// Navigation
		public ICollection<MaintenanceHistory>? MaintenanceHistory { get; set; }
	}
}
