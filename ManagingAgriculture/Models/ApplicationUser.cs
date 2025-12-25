using Microsoft.AspNetCore.Identity;

namespace ManagingAgriculture.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? CompanyId { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("CompanyId")]
        public Company? Company { get; set; }

        // Optionally add FirstName/LastName if you want better profile data
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
