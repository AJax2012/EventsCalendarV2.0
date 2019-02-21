using System.Web.Mvc;

namespace EventsCalendar.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Performances");
        }
    }
}