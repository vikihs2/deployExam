using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class MachineryController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Machinery Management";
            return View();
        }
    }
}
