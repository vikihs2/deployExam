using Microsoft.AspNetCore.Mvc;

namespace ManagingAgriculture.Controllers
{
    public class MachineryController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Machinery Management";
            return View();
        }
    }
}
