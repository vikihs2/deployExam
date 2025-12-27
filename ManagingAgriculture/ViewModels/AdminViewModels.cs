using System.Collections.Generic;
using ManagingAgriculture.Models;

namespace ManagingAgriculture.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalPlants { get; set; }
        public int TotalResources { get; set; }
        public int TotalMachinery { get; set; }
        
        // Role Distribution
        public Dictionary<string, int> UserRolesCount { get; set; } = new Dictionary<string, int>();

        public List<ApplicationUser> RecentUsers { get; set; } = new List<ApplicationUser>();
        public List<Company> RecentCompanies { get; set; } = new List<Company>();
    }

    public class UserManagementViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CurrentRole { get; set; }
        public string CompanyName { get; set; }
        public bool IsLockedOut { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }

    public class CompanyOverviewViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerEmail { get; set; }
        public int StaffCount { get; set; }
        public int PlantCount { get; set; }
    }
}
