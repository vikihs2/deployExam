using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagingAgriculture.Controllers
{
    [Authorize(Roles = "ITSupport")]
    public class ITSupportController : Controller
    {
        private readonly ManagingAgriculture.Data.ApplicationDbContext _context;
        public ITSupportController(ManagingAgriculture.Data.ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
             var messages = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(_context.ContactForms.OrderByDescending(m => m.CreatedDate));
            return View(messages);
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
                msg.RepliedBy = "IT Support"; 
                
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
