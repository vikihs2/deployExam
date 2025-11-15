using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
	public class Sensor
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[StringLength(20)]
		public string SensorType { get; set; } = string.Empty;

		[StringLength(100)]
		public string? Location { get; set; }

		[Column(TypeName = "date")]
		public DateTime InstallationDate { get; set; }

		public DateTime CreatedDate { get; set; }

		// Navigation
		public ICollection<SensorReading>? SensorReadings { get; set; }
	}
}
