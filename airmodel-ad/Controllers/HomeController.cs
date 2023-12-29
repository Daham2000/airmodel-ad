using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using airmodel_ad.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using airmodel_ad.Business.Interface;
using System.Security.Claims;
using System.Diagnostics.Metrics;
using System.IO;
using airmodel_ad.Business.Services;
using airmodel_ad.Models.ParamModels;
using airmodel_ad.Models.Chart;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace airmodel_ad.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        List<ProductModel> productModels;
        List<ProductModel> filteredProductModels;
        List<ProductModel> searchModels;
        List<ProductModel> selectedCategory;
        List<CartItemModel> cartModels;
        List<Category> categories;
        List<OrderModel> orderModels;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;
        int total = 0;
        int additational = 0;
        string selectedImage = string.Empty;
        Guid VarientId = Guid.Empty;
        Guid productId = Guid.Empty;

        public HomeController(IUserService user, IProductService product, ICartService cartService, ICategoryService categoryService, IOrderService orderService)
        {
            this.userService = user;
            this.productService = product;
            this.cartService = cartService;
            this.categoryService = categoryService;
            this.orderService = orderService;
        }

        private async Task<bool> GetHomePageData()
        {
            try {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                productModels = productService.GetAllProducts();
                filteredProductModels = productService.GetAllAvailableProducts();
                categories = categoryService.GetAllCategories();
                selectedCategory = productService.GetAllProductsByCategory(categories.FirstOrDefault().categoryId);
                CartModel cartModel = cartService.GetCart(emailValue);

                cartModels = cartService.GetCartItem(cartModel.cartId);
                
                foreach (var item in cartModels)
                {
                    total += item.total;
                    additational += item.additionalCost;
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
                Debug.WriteLine("Loading Index");
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = userService.GetUserByEmail(emailValue);
                Debug.WriteLine(user.userRole == "admin");
                Debug.WriteLine(user.userRole);
                if (user.userRole == "admin")
                {
                    DateTime currentMonthDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime currentYearDateTime = new DateTime(DateTime.Now.Year, 1, 1);
                    List<User> users = userService.GetAllUsers();
                    List<OrderModel> pendingOrders = orderService.GetOrderByDateTime(currentMonthDateTime);
                    List<OrderModel> orderPerYear = orderService.GetOrderByDateTime(currentYearDateTime);
                    for (int i = 0; i <= pendingOrders.Count() - 1; i++)
                    {
                        pendingOrders[i].orderItems = orderService.GetAllOrderItems(pendingOrders[i].oId);
                        if (pendingOrders[i].orderStatus == "0")
                        {
                            pendingOrders[i].orderStatus = "Pending";
                        }
                        else if (pendingOrders[i].orderStatus == "1")
                        {
                            pendingOrders[i].orderStatus = "Shipped";
                        }
                        else
                        {
                            pendingOrders[i].orderStatus = "Delivered";
                        }
                    }
                    Debug.WriteLine(orderPerYear.Count());
                    Debug.WriteLine("Here");
                    for (int i = 0; i <= orderPerYear.Count() - 1; i++)
                    {
                        orderPerYear[i].orderItems = orderService.GetAllOrderItems(orderPerYear[i].oId);
                        if (orderPerYear[i].orderStatus == "0")
                        {
                            orderPerYear[i].orderStatus = "Pending";
                        }
                        else if (orderPerYear[i].orderStatus == "1")
                        {
                            orderPerYear[i].orderStatus = "Shipped";
                        }
                        else
                        {
                            orderPerYear[i].orderStatus = "Delivered";
                        }
                    }
                    ViewBag.orders = pendingOrders;
                    ViewBag.ordersC = pendingOrders.Count();
                    ViewBag.orderPerYear = orderPerYear;
                    ViewBag.orderPerYearC = orderPerYear.Count();
                    ViewBag.usersC = users.Count();

                    List<DataPoint> dataPoints = new List<DataPoint>();
                    List<DataPoint> orderPerYearDataPoints = new List<DataPoint>();
                    Debug.WriteLine("Here 2");

                    for (int i = 0; i <= pendingOrders.Count() - 1; i++)
                    {
                        dataPoints.Add(new DataPoint("Month - " + pendingOrders[i].orderTime.Month.ToString() + ": Day - " + pendingOrders[i].orderTime.Day.ToString(), pendingOrders[i].total));
                    }
                    for (int i = 0; i <= orderPerYear.Count() - 1; i++)
                    {
                        orderPerYearDataPoints.Add(new DataPoint(orderPerYear[i].orderTime.Month.ToString() + ":" + orderPerYear[i].orderTime.Day.ToString(), orderPerYear[i].total));
                    }
                    Debug.WriteLine("Here 3");

                    ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                    ViewBag.OrderPerYearDataPoints = JsonConvert.SerializeObject(orderPerYearDataPoints);

                    return View("../Admin/AdminView");
                }
                else
                {
                    bool re = await GetHomePageData();

                    ViewBag.productModels = productModels;
                    ViewBag.filteredProductModels = filteredProductModels;
                    ViewBag.selectedCategory = selectedCategory;
                    ViewBag.cartModels = cartModels;
                    ViewBag.len = cartModels.Count();
                    ViewBag.total = total;
                    ViewBag.additational = additational;
                    ViewBag.categories = categories;
                    return View("../Home/HomeView");
                }
            } catch(Exception ex)
            {
                return View("../Home/HomeView");
            }
        }

        public async Task<IActionResult> FilterProducts(int value) {
            await GetHomePageData();
            if(value == 1)
            {
                filteredProductModels = productService.GetAllAvailableProducts();
            } else
            {
                filteredProductModels = productService.GetAllUnavailableProducts();
            }
            ViewBag.productModels = productModels;
            ViewBag.filteredProductModels = filteredProductModels;
            ViewBag.selectedCategory = selectedCategory;
            ViewBag.cartModels = cartModels;
            ViewBag.len = cartModels.Count();
            ViewBag.total = total;
            ViewBag.additational = additational;

            ViewBag.categories = categories;
            return View("../Home/HomeView");
        }

        public async Task<IActionResult> GetAllMyOrdera()
        {
            try
            {
                bool re = await GetHomePageData();
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = userService.GetUserByEmail(emailValue);
         
                orderModels = orderService.GetAllOrders(user.userId);

                for (int i=0; i<=orderModels.Count()-1; i++) {
                    orderModels[i].orderItems = orderService.GetAllOrderItems(orderModels[i].oId);
                    Debug.WriteLine("orderModels[i].orderStatus");
                    Debug.WriteLine(orderModels[i].orderStatus);
                    if (orderModels[i].orderStatus == "0")
                    {
                        orderModels[i].orderStatus = "Pending";
                    }
                    else if (orderModels[i].orderStatus == "1")
                    {
                        orderModels[i].orderStatus = "Shipped";
                    }
                    else
                    {
                        orderModels[i].orderStatus = "Delivered";
                    }
                }
                Debug.WriteLine(user.userId);
                Debug.WriteLine(orderModels.Count());

                ViewBag.orderModels = orderModels;

                ViewBag.productModels = productModels;
                ViewBag.selectedCategory = selectedCategory;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();
                ViewBag.total = total;
                ViewBag.categories = categories;
                ViewBag.additational = additational;

                return View("../Home/MyOrderView");
            }
            catch (Exception ex)
            {
                return View("../Home/MyOrderView");
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
                if(product.hasVarients == false)
                {
                    ViewBag.selectedImage = product.productImage;
                    ViewBag.VarientId = "Null";
                    ViewBag.selectedProPrice = product.productBasicPrice;
                }
                else
                {
                    ViewBag.selectedImage = product.varientOptionModels[0].varientImage;
                    ViewBag.VarientId = product.varientOptionModels[0].varientOptionId;
                    ViewBag.selectedProPrice = product.varientOptionModels[0].varientPrice;
                }

                ViewBag.additational = additational;

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
                total = 0;
                Debug.WriteLine("VarientId: ");
                Debug.WriteLine(VarientId);
                Debug.WriteLine("productId: ");
                Debug.WriteLine(productId);
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = userService.GetUserByEmail(emailValue);
                ProductModel product = productService.GetProductById(productId);
                bool ifAvailable = cartService.CheckProductAvailableInCart(product, user.userId);
                int additionalCost = 0;
                if(VarientId != "Null")
                {
                    VarientOptionModel varientOptionModel = productService.GetProductVarientById(new Guid(VarientId));
                    ViewBag.selectedProPrice = varientOptionModel.varientPrice;
                    ViewBag.VarientId = new Guid(VarientId);
                    total = (varientOptionModel.varientPrice + product.productBasicPrice) * qty;
                    additionalCost = varientOptionModel.varientPrice * qty;
                } else
                {
                    total = product.productBasicPrice * qty;
                    ViewBag.VarientId = "Null";
                    ViewBag.selectedProPrice = product.productBasicPrice;
                }
                CartModel cartModel = cartService.GetCart(emailValue);
                Debug.WriteLine("total: ");
                Debug.WriteLine(total);

                CartItemModel newCartItemModel = new CartItemModel();
                newCartItemModel.cartItemId = Guid.NewGuid();
                newCartItemModel.cartId = cartModel.cartId;
                newCartItemModel.productId = productId;
                newCartItemModel.qty = qty;
                newCartItemModel.total = total;
                newCartItemModel.additionalCost = additionalCost;
                if (ifAvailable == false)
                {
                    if (VarientId != "Null")
                    {

                        newCartItemModel.varientOptionId = new Guid(VarientId);
                        bool result = cartService.AddCart(newCartItemModel);
                        
                    }
                    else
                    {
                        newCartItemModel.varientOptionId = null;
                        if(product.hasVarients == false)
                        {
                            bool result = cartService.AddCart(newCartItemModel);
                        }
                    }
                }
                total = 0;
                await GetHomePageData();
                
                ViewBag.productModels = productModels;
                ViewBag.selectedCategory = selectedCategory;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();
                ViewBag.total = total;
                ViewBag.categories = categories;
                ViewBag.additational = additational;

                return View("../Home/CheckOutView");
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
                ViewBag.filteredProductModels = filteredProductModels;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();
                ViewBag.total = total;
                ViewBag.categories = categories;
                ViewBag.VarientId = "Null";
                ViewBag.additational = additational;

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
                VarientOptionModel varientOptionModel = productService.GetProductVarientById(imageSelected);
                if(varientOptionModel == null) {
                    ProductModel productModel = productService.GetProductById(imageSelected);
                    ViewBag.selectedImage = productModel.productImage;
                    ViewBag.VarientId = "Null";
                }
                else
                {
                    ViewBag.selectedImage = varientOptionModel.varientImage;
                    ViewBag.VarientId = imageSelected;
                }

                return PartialView("../Product/ImageView");
            }
            catch (Exception ex)
            {
                return PartialView("../Product/ImageView");
            }
        }

        public async Task<PartialViewResult> SetVarientID(Guid imageSelected)
        {
            try
            {
                VarientOptionModel varientOptionModel = productService.GetProductVarientById(imageSelected);
                if (varientOptionModel == null)
                {
                    ViewBag.VarientId = "Null";
                }
                else
                {
                    ViewBag.VarientId = imageSelected;
                }

                return PartialView("../Product/VarientIdView");
            }
            catch (Exception ex)
            {
                return PartialView("../Product/VarientIdView");
            }
        }

        public async Task<PartialViewResult> SelectImageChangePrice(Guid imageSelected)
        {
            try
            {
                Debug.WriteLine("imageSelected");
                Debug.WriteLine(imageSelected);
                VarientOptionModel varientOptionModel = productService.GetProductVarientById(imageSelected);
                if (varientOptionModel == null)
                {
                    ProductModel productModel = productService.GetProductById(imageSelected);
                    ViewBag.selectedProPrice = productModel.productBasicPrice;
                    Debug.WriteLine("productBasicPrice");
                    ViewBag.VarientId = "Null";
                    Debug.WriteLine(productModel.productBasicPrice);

                }
                else
                {
                    ViewBag.selectedProPrice = varientOptionModel.varientPrice;
                    Debug.WriteLine("varientPrice");
                    ViewBag.VarientId = imageSelected;
                    Debug.WriteLine(varientOptionModel.varientPrice);

                }

                return PartialView("../Product/ProductPriceComponent");
            }
            catch (Exception ex)
            {
                return PartialView("../Product/ProductPriceComponent");
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
                ViewBag.additational = additational;

                return View("../Home/CheckOutView");
            }
            catch (Exception ex)
            {
                return View("../Home/CheckOutView");
            }
        }

        public IActionResult OpenEditUser()
        {
            string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = userService.GetUserByEmail(emailValue);
            ViewBag.user = user;
            return View("../Home/MyProfile");
        }

        public IActionResult EditUserAction(Guid userId, string userName, string userEmail, string password)
        {
            User user = userService.GetUserByUid(userId);
            user.userName = userName;
            user.userEmail = userEmail;
            user.userPassword = password;
            userService.EditUser(user.userId, user);
            user = userService.GetUserByUid(userId);
            ViewBag.user = user;

            return View("../Home/MyProfile");
        }

    }
}