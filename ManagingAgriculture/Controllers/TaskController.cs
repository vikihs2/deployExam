using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagingAgriculture.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ManagingAgriculture.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ManagingAgriculture.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var tasks = await _context.TaskAssignments
                .Where(t => t.AssignedToUserId == user.Id)
                .OrderByDescending(t => t.AssignedDate)
                .ToListAsync();

            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var task = await _context.TaskAssignments.FindAsync(id);

            if (task != null && task.AssignedToUserId == user.Id)
            {
                task.IsCompletedByEmployee = true;
                // If boss auto-approval is not required, we stop here. 
                // Currently Boss needs to approve.
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
