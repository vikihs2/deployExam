using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class ResourcesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Resource Management";
            return View();
        }
    }
}
