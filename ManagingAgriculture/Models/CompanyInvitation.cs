using System;

namespace ManagingAgriculture.Models
{
    public class CompanyInvitation
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public required string Role { get; set; } // "Manager", "Employee"
        public string Token { get; set; } = string.Empty;
        public bool IsUsed { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Job Offer Details
        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public int LeaveDays { get; set; } = 20;
    }
}
