using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
	public class Plant
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[StringLength(50)]
		public string PlantType { get; set; } = string.Empty;

		[Column(TypeName = "date")]
		public DateTime PlantedDate { get; set; }

		[Column(TypeName = "date")]
		public DateTime? ExpectedHarvestDate { get; set; }

		[Range(0, 100)]
		public int GrowthStagePercent { get; set; }

		[StringLength(200)]
		public string? NextTask { get; set; }

		public string? Notes { get; set; }

		[StringLength(100)]
		public string? Location { get; set; }

		[StringLength(20)]
		public string Status { get; set; } = "Active";

		// Agronomic fields
		[StringLength(50)]
		public string? SoilType { get; set; }

		[StringLength(50)]
		public string? SunlightExposure { get; set; }

		public bool IsIndoor { get; set; }

		[Range(-50, 60)]
		public decimal? AvgTemperatureCelsius { get; set; }

		[Range(0, 365)]
		public int? WateringFrequencyDays { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }

		// Navigation
		public ICollection<ResourceUsage>? ResourceUsages { get; set; }
	}
}
