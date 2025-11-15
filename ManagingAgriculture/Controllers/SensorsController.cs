using Microsoft.AspNetCore.Mvc;

namespace ManagingAgriculture.Controllers
{
    public class SensorsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Soil Humidity Monitor";
            return View();
        }
    }
}
