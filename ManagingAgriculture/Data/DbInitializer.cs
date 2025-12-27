using Microsoft.AspNetCore.Identity;
using ManagingAgriculture.Models;

namespace ManagingAgriculture.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            string[] roleNames = { "SystemAdmin", "ITSupport", "Boss", "Manager", "Employee", "User" };
            
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var itEmail = "it@gmail.com";
            if (await userManager.FindByEmailAsync(itEmail) == null)
            {
                var itUser = new ApplicationUser { UserName = "it_support", Email = itEmail, EmailConfirmed = true };
                var res = await userManager.CreateAsync(itUser, "It123!"); 
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(itUser, "ITSupport");
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = "admin", Email = adminEmail, EmailConfirmed = true };
                var res = await userManager.CreateAsync(adminUser, "Admin123!"); 
                if (!res.Succeeded)
                {
                   // Log error or handle? For now, assume success or already exists.
                }
            }
            
            // ALWAYS ensure admin has the role
            if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "SystemAdmin"))
            {
                await userManager.AddToRoleAsync(adminUser, "SystemAdmin");
            }
        }
    }
}
