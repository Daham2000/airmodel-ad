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
        List<ProductModel> searchModels;
        List<ProductModel> selectedCategory;
        List<CartItemModel> cartModels;
        List<Category> categories;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly ICategoryService categoryService;
        int total = 0;
        string selectedImage = string.Empty;

        public HomeController(IUserService user, IProductService product, ICartService cartService, ICategoryService categoryService)
        {
            this.userService = user;
            this.productService = product;
            this.cartService = cartService;
            this.categoryService = categoryService;
        }

        private async Task<bool> GetHomePageData()
        {
            try {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                productModels = productService.GetAllProducts();
                categories = categoryService.GetAllCategories();
                selectedCategory = productService.GetAllProductsByCategory(categories.FirstOrDefault().categoryId);
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
                ViewBag.selectedCategory = selectedCategory;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();
                ViewBag.total = total;
                ViewBag.categories = categories;

                return View("../Home/HomeView");
            } catch(Exception ex)
            {
                return View("../Home/HomeView");
            }
        }

        public async Task<PartialViewResult> GetOnSaleItems()
        {
            try
            {
                List<ProductModel> availableProductList = productService.GetAllAvailableProducts();
                Debug.WriteLine("GetOnSaleItems");

                ViewBag.availableProductList = availableProductList;
                return PartialView("../Home/ProductHList");
            }
            catch (Exception ex)
            {
                return PartialView("../Home/ProductHList");
            }
        }

        public async Task<PartialViewResult> GetProductByCategory(Guid categoryId)
        {
            try
            {
                selectedCategory = productService.GetAllProductsByCategory(categoryId);

                ViewBag.selectedCategory = selectedCategory;

                Debug.WriteLine("GetOnSaleItems");
                Debug.WriteLine(categoryId);
                return PartialView("../Home/CategoryItemProduct");
            }
            catch (Exception ex)
            {
                return PartialView("../Home/CategoryItemProduct");
            }
        }

        public async Task<PartialViewResult> SearchProduct(string searchInput)
        {
            try
            {
                Debug.WriteLine("searchInput");
                Debug.WriteLine(searchInput);
                searchModels = productService.GetSearchProduct(searchInput);
                Debug.WriteLine(searchModels.Count());
                ViewBag.searchModels = searchModels;

                return PartialView("../Home/CategoryItemProduct");
            }
            catch (Exception ex)
            {
                return PartialView("../Home/CategoryItemProduct");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> ViewProduct(Guid searchInput)
        {
            try
            {
                bool re = await GetHomePageData();
                ProductModel product = productService.GetProductById(searchInput);
                ViewBag.product = product;
                ViewBag.selectedImage = product.productImage;

                ViewBag.productModels = productModels;
                ViewBag.selectedCategory = selectedCategory;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();
                ViewBag.total = total;
                ViewBag.categories = categories;

                return View("../Product/ProductView");
            }
            catch (Exception ex)
            {
                return View("../Product/ProductView");
            }
        }

    }
}