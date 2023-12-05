using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using airmodel_ad.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using airmodel_ad.Business.Interface;

namespace airmodel_ad.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        List<ProductModel> productModels;
        private readonly IUserService userService;
        private readonly IProductService productService;

        public HomeController(IUserService user, IProductService product)
        {
            this.userService = user;
            this.productService = product;
        }

        public IActionResult Index()
        {
            productModels = productService.GetAllProducts();
            ViewBag.productModels = productModels;
            return View("../Home/HomeView");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Auth");
        }

    }
}