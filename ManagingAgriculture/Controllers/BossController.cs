using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagingAgriculture.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagingAgriculture.Controllers
{
    [Authorize(Roles = "Boss")]
    public class BossController : Controller
    {
        private readonly ManagingAgriculture.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BossController(ManagingAgriculture.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> ManageCompany()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CompanyId == null) return NotFound("Company not found.");
            var company = await _context.Companies.FindAsync(user.CompanyId);
            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompany(int id, string name, Microsoft.AspNetCore.Http.IFormFile? logo)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                company.Name = name;
                
                if (logo != null && logo.Length > 0)
                {
                    // Save Logo
                    var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(logo.FileName);
                    var filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot/images/companies", fileName);
                    
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath)!);
                    
                    using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                    {
                        await logo.CopyToAsync(stream);
                    }
                    company.LogoPath = "/images/companies/" + fileName;
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageCompany));
        }

        public async Task<IActionResult> ManageStaff()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CompanyId == null) return NotFound("Company not found.");

            // Get Employees and Managers (exclude self)
            var staff = await _userManager.GetUsersInRoleAsync("Employee");
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            
            // Filter by CompanyId (Note: GetUsersInRoleAsync does not filter by company, so we filter in memory or join)
            // Efficient way: Query users directly
            var companyUsers = _context.Users.Where(u => u.CompanyId == user.CompanyId && u.Id != user.Id).ToList();
            
            // Also get pending invitations
            ViewBag.Invitations = _context.CompanyInvitations.Where(i => i.CompanyId == user.CompanyId && !i.IsUsed).ToList();

            return View(companyUsers);
        }

        [HttpPost]
        public async Task<IActionResult> InviteStaff(string email, string role)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.CompanyId == null) return Challenge();

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                // User exists: Update them
                existingUser.CompanyId = currentUser.CompanyId;
                await _userManager.UpdateAsync(existingUser);
                
                // Remove old roles and add new one
                var roles = await _userManager.GetRolesAsync(existingUser);
                await _userManager.RemoveFromRolesAsync(existingUser, roles);
                await _userManager.AddToRoleAsync(existingUser, role);
            }
            else
            {
                // Create Invitation
                var invite = new CompanyInvitation
                {
                    Email = email,
                    CompanyId = currentUser.CompanyId.Value,
                    Role = role,
                    Token = Guid.NewGuid().ToString() // Simple token, checking mainly email
                };
                _context.CompanyInvitations.Add(invite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageStaff));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveStaff(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.CompanyId = null;
                await _userManager.UpdateAsync(user);
                // Optionally reset role to "User" or leave as is? Better to reset to User.
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, "User");
            }
            return RedirectToAction(nameof(ManageStaff));
        }

        [HttpPost]
        public async Task<IActionResult> PromoteStaff(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, newRole);
            }
            return RedirectToAction(nameof(ManageStaff));
        }
    }
}
