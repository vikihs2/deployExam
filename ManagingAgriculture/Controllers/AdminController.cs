using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ManagingAgriculture.Models;
using ManagingAgriculture.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ManagingAgriculture.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class AdminController : Controller
    {
        private readonly ManagingAgriculture.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ManagingAgriculture.Data.ApplicationDbContext context, 
                               UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 1. Dashboard
        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalCompanies = await _context.Companies.CountAsync(),
                TotalPlants = await _context.Plants.CountAsync(),
                TotalResources = await _context.Resources.CountAsync(),
                TotalMachinery = await _context.Machinery.CountAsync(),
                RecentUsers = await _userManager.Users.OrderByDescending(u => u.Id).Take(5).ToListAsync(), // Approximate "recent" by ID if no Date
                RecentCompanies = await _context.Companies.OrderByDescending(c => c.Id).Take(5).ToListAsync()
            };

            // Calculate Role Stats
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                model.UserRolesCount.Add(role, usersInRole.Count);
            }

            return View(model);
        }

        // 2. User Management
        public async Task<IActionResult> Users(string search, string filterRole)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Email.Contains(search) || u.UserName.Contains(search) || u.FirstName.Contains(search));
            }

            var users = await query.ToListAsync();
            var userViewModels = new List<UserManagementViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                string companyName = "Freelancer";
                if (user.CompanyId != null)
                {
                    var comp = await _context.Companies.FindAsync(user.CompanyId);
                    companyName = comp?.Name ?? "Unknown";
                }

                userViewModels.Add(new UserManagementViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}",
                    CurrentRole = roles.FirstOrDefault() ?? "None",
                    CompanyName = companyName,
                    IsLockedOut = await _userManager.IsLockedOutAsync(user)
                });
            }

            if (!string.IsNullOrEmpty(filterRole))
            {
                userViewModels = userViewModels.Where(u => u.CurrentRole == filterRole).ToList();
            }

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentRole = filterRole;
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(userViewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (await _userManager.IsLockedOutAsync(user))
            {
                await _userManager.SetLockoutEndDateAsync(user, null); // Unlock
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, System.DateTimeOffset.MaxValue); // Lock
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(string id, string newRole)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, newRole);

            return RedirectToAction(nameof(Users));
        }

        // 3. Company Overview
        public async Task<IActionResult> Companies()
        {
            var companies = await _context.Companies.ToListAsync();
            var model = new List<CompanyOverviewViewModel>();

            foreach (var c in companies)
            {
                var owner = await _userManager.FindByIdAsync(c.CreatedByUserId ?? "");
                var staffCount = await _userManager.Users.CountAsync(u => u.CompanyId == c.Id);
                var plantCount = await _context.Plants.CountAsync(p => p.CompanyId == c.Id);

                model.Add(new CompanyOverviewViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    OwnerEmail = owner?.Email ?? "Unknown",
                    StaffCount = staffCount,
                    PlantCount = plantCount
                });
            }
            return View(model);
        }

        // 4. Contact Messages & Reports
        public async Task<IActionResult> Messages()
        {
            var messages = await _context.ContactForms.OrderByDescending(m => m.CreatedDate).ToListAsync();
            return View(messages);
        }

        public IActionResult Reports()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyToMessage(int id, string replyContent)
        {
            var msg = await _context.ContactForms.FindAsync(id);
            if (msg != null)
            {
                msg.ReplyMessage = replyContent;
                msg.IsReplied = true;
                msg.RepliedDate = System.DateTime.UtcNow;
                msg.RepliedBy = "SystemAdmin";
                
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Messages)); // Redirect back to Messages tab
        }
    }
}
