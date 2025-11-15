using Microsoft.AspNetCore.Mvc;

namespace ManagingAgriculture.Controllers
{
    public class ResourcesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Resource Management";
            return View();
        }
    }
}
