using Microsoft.AspNetCore.Mvc;

namespace SimpleLinkShrinkLibrary.Tests.Frontend.SqlServerWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
