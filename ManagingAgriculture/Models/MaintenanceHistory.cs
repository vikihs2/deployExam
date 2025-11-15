using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
    public class MaintenanceHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MachineryId { get; set; }

        [Column(TypeName = "date")]
        public DateTime ServiceDate { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Cost { get; set; }

        public string? Notes { get; set; }

        // Navigation
        public Machinery? Machinery { get; set; }
    }
}