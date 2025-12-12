using Microsoft.AspNetCore.Mvc;

namespace ManagingAgriculture.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "AgroCore - Agriculture Management";
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact - AgroCore";
            return View();
        }
    }
}
