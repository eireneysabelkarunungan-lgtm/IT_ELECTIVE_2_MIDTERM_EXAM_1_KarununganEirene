using Microsoft.AspNetCore.Mvc;

namespace VehicleServiceMonitoringSystem.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return RedirectToAction("Index", "ServiceJob");
            }

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
