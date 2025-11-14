using Microsoft.AspNetCore.Mvc;

namespace ManagingAgriculture.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            return View();
        }
    }
}
