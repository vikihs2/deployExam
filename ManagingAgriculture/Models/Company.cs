using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? LogoPath { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The User Id of the Boss who created the company.
        /// </summary>
        public string CreatedByUserId { get; set; } = string.Empty;

        // Navigation property for employees (including the Boss)
        public ICollection<ApplicationUser> Employees { get; set; } = new List<ApplicationUser>();
    }
}
