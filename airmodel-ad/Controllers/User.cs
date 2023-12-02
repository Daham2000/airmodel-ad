using Microsoft.AspNetCore.Mvc;
using airmodel_ad.Data;

namespace airmodel_ad.Controllers
{
    public class User : Controller
    {
        private readonly AppDbContext _appDbContext;

        public User(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
