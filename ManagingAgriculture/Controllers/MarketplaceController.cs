using Microsoft.AspNetCore.Mvc;

namespace ManagingAgriculture.Controllers
{
    public class MarketplaceController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Marketplace";
            return View();
        }
    }
}
