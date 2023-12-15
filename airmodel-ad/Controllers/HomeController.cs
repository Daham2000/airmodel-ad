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
        Guid VarientId = Guid.Empty;
        Guid productId = Guid.Empty;

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

                cartModels = cartService.GetCartItem(cartModel.cartId);

                foreach (var item in cartModels)
                {
                    if (item.varientOptionId.ToString() != "00000000-0000-0000-0000-000000000000")
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

        [HttpPost]
        public async Task<IActionResult> AddToCart(int qty, Guid productId, string VarientId)
        {
            try
            {
                ProductModel product = productService.GetProductById(productId);
                bool ifAvailable = cartService.CheckProductAvailableInCart(product);

                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                CartModel cartModel = cartService.GetCart(emailValue);
                Debug.WriteLine("qty");
                Debug.WriteLine(qty);


                CartItemModel newCartItemModel = new CartItemModel();
                newCartItemModel.cartItemId = Guid.NewGuid();
                newCartItemModel.cartId = cartModel.cartId;
                newCartItemModel.productId = productId;
                newCartItemModel.qty = qty;
                if (ifAvailable == false)
                {
                    if (VarientId != "Null")
                    {

                        newCartItemModel.varientOptionId = new Guid(VarientId);
                    } else
                    {
                        newCartItemModel.varientOptionId = Guid.Empty;
                    }
                    bool result = cartService.AddCart(newCartItemModel);
                }
                await GetHomePageData();
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

        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            try
            {
                bool re = await GetHomePageData();
                Debug.WriteLine("cartItemId");
                Debug.WriteLine(cartModels.Count());
                CartItemModel cartItem = cartModels.Where((cart) => cart.cartItemId == cartItemId).FirstOrDefault();
                Debug.WriteLine(cartItem.cartItemId);
                cartService.RemoveCartItem(cartItem);
                await GetHomePageData();

                ViewBag.productModels = productModels;
                ViewBag.selectedCategory = selectedCategory;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();
                ViewBag.total = total;
                ViewBag.categories = categories;

                return View("../Home/HomeView");
            }
            catch (Exception ex)
            {
                return View("../Home/HomeView");
            }
        }

        public async Task<PartialViewResult> SelectImage(Guid imageSelected)
        {
            try
            {
                Debug.WriteLine("imageSelected");
                Debug.WriteLine(imageSelected);
                VarientOptionModel varientOptionModel = productService.GetProductVarientById(imageSelected);
                ViewBag.selectedImage = varientOptionModel.varientImage;
                
                return PartialView("../Product/ImageView");
            }
            catch (Exception ex)
            {
                return PartialView("../Product/ImageView");
            }
        }

        public async Task<IActionResult> LoadCheckOutView()
        {
            try
            {
                Debug.WriteLine("LoadCheckOutView");
                await GetHomePageData();

                ViewBag.productModels = productModels;
                ViewBag.selectedCategory = selectedCategory;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();
                ViewBag.total = total;
                ViewBag.categories = categories;

                return View("../Home/CheckOutView");
            }
            catch (Exception ex)
            {
                return View("../Home/CheckOutView");
            }
        }

    }
}