using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
    public class ResourceUsage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResourceId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal QuantityUsed { get; set; }

        public DateTime UsageDate { get; set; }

        public int? PlantId { get; set; }

        [StringLength(100)]
        public string? FieldName { get; set; }

        public string? Notes { get; set; }

        // Navigation
        public Resource? Resource { get; set; }
        public Plant? Plant { get; set; }
    }
}