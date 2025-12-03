using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class SensorsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Soil Humidity Monitor";
            return View();
        }
    }
}
