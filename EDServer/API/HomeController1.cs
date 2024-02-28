using Microsoft.AspNetCore.Mvc;

namespace EDServer.API
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
