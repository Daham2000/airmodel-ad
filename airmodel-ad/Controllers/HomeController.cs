using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using airmodel_ad.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using airmodel_ad.Business.Interface;
using System.Security.Claims;

namespace airmodel_ad.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        List<ProductModel> productModels;
        List<CartItemModel> cartModels;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        int total = 0;

        public HomeController(IUserService user, IProductService product, ICartService cartService)
        {
            this.userService = user;
            this.productService = product;
            this.cartService = cartService;
        }

        private async Task<bool> GetHomePageData()
        {
            try {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                productModels = productService.GetAllProducts();
                CartModel cartModel = cartService.GetCart(emailValue);
                Debug.WriteLine("cartModel: ");
                Debug.WriteLine(cartModel.cartId);

                cartModels = cartService.GetCartItem(cartModel.cartId);

                foreach (var item in cartModels)
                {
                    if (item.varientOptionId != null)
                    {
                        total += item.varientOption.varientPrice;
                    }
                    else
                    {
                        total += item.products.productBasicPrice;
                    }
                }
                return true;
            } catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<IActionResult> Index()
        {
            try {
                bool re = await GetHomePageData();
                ViewBag.productModels = productModels;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();

                ViewBag.total = total;
                return View("../Home/HomeView");
            } catch(Exception ex)
            {
                return View("../Home/HomeView");
            }
        }

        public async Task<IActionResult> GetOnSaleItems()
        {
            try
            {
                bool re = await GetHomePageData();
                List<ProductModel> availableProductList = productService.GetAllAvailableProducts();
                ViewBag.productModels = productModels;
                ViewBag.availableProductList = availableProductList;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();

                ViewBag.total = total;
                return View("../Home/HomeView");
            }
            catch (Exception ex)
            {
                ViewBag.productModels = productModels;
                ViewBag.cartModels = cartModels;
                ViewBag.len = 0;
                return View("../Home/HomeView");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Auth");
        }

    }
}