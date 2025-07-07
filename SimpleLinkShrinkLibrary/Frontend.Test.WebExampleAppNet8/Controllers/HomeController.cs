using Microsoft.AspNetCore.Mvc;

namespace SimpleLinkShrinkLibrary.Frontend.Test.WebExampleAppNet8.Controllers
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
