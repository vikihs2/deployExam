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
        public required string Token { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
