using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ManagingAgriculture.Controllers
{
    [Authorize]
    public class MarketplaceController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Marketplace";
            return View();
        }
    }
}
