using Microsoft.AspNetCore.Mvc;

namespace airmodel_ad.Controllers
{
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
