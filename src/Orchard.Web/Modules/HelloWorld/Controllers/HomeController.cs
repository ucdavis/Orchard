using System.Web.Mvc;
using Orchard.Themes;

namespace HelloWorld.Controllers
{
    public class HomeController : Controller
    {
        [Themed]
        public ActionResult Index() {
            return View("Hello World!");
        }

    }
}