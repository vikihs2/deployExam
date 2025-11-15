using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
	public class SensorReading
	{
		[Key]
		public long Id { get; set; }

		[Required]
		public int SensorId { get; set; }

		public DateTime Timestamp { get; set; }

		[Column(TypeName = "decimal(5,2)")]
		public decimal? HumidityPercent { get; set; }

		[Column(TypeName = "decimal(5,2)")]
		public decimal? TemperatureC { get; set; }

		// Navigation
		public Sensor? Sensor { get; set; }
	}
}
