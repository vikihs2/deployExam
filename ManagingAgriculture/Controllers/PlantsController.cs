using Microsoft.AspNetCore.Mvc;

namespace ManagingAgriculture.Controllers
{
    public class PlantsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Plant Tracking";
            return View();
        }
    }
}
