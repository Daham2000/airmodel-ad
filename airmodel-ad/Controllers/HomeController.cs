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
        List<OrderModel> orderModels;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;
        int total = 0;
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
                categories = categoryService.GetAllCategories();
                selectedCategory = productService.GetAllProductsByCategory(categories.FirstOrDefault().categoryId);
                CartModel cartModel = cartService.GetCart(emailValue);

                cartModels = cartService.GetCartItem(cartModel.cartId);
                Debug.WriteLine("cartModels");
                Debug.WriteLine(cartModels.Count());
                foreach (var item in cartModels)
                {
                    if (item.varientOptionId.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        total += item.varientOption.varientPrice * item.qty;
                    }
                    else
                    {
                        total += item.products.productBasicPrice * item.qty;
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
                Debug.WriteLine("Loading Index");
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = userService.GetUserByEmail(emailValue);
                if (user.userRole == "admin")
                {
                    List<OrderModel> pendingOrders = orderService.GetAllPendingOrders();
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
                    ViewBag.orders = pendingOrders;

                    List<DataPoint> dataPoints = new List<DataPoint>();
                    for (int i = 0; i < pendingOrders.Count; i++)
                    {
                        dataPoints.Add(new DataPoint(pendingOrders[i].fName.ToString(), pendingOrders[i].total));
                    }

                    ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

                    return View("../Admin/AdminView");
                }
                else
                {
                    bool re = await GetHomePageData();

                    ViewBag.productModels = productModels;
                    ViewBag.selectedCategory = selectedCategory;
                    ViewBag.cartModels = cartModels;
                    ViewBag.len = cartModels.Count();
                    ViewBag.total = total;
                    ViewBag.categories = categories;
                    return View("../Home/HomeView");
                }
            } catch(Exception ex)
            {
                return View("../Home/HomeView");
            }
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
                if(varientOptionModel == null) {
                    ProductModel productModel = productService.GetProductById(imageSelected);
                    ViewBag.selectedImage = productModel.productImage;
                    ViewBag.selectedProPrice = productModel.productBasicPrice;

                }
                else
                {
                    ViewBag.selectedImage = varientOptionModel.varientImage;
                    ViewBag.selectedProPrice = varientOptionModel.varientPrice;

                }

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

        public async Task<IActionResult> PlaceOrder(string fName, string lName, string orderNote, string county, string street, string houseNo, string city, string postCode, string phoneNumber)
        {
            try
            {
                await GetHomePageData();
                Debug.WriteLine("Fname");
                Debug.WriteLine(fName);
                Debug.WriteLine(street);
                Debug.WriteLine(postCode);
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = userService.GetUserByEmail(emailValue);
                OrderModel orderModel = new OrderModel();
                orderModel.oId = new Guid();
                total = 0;
                foreach (var item in cartModels)
                {
                    if (item.varientOptionId.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        total += item.varientOption.varientPrice * item.qty;
                    }
                    else
                    {
                        total += item.products.productBasicPrice * item.qty;
                    }
                }
                orderModel.total = total;
                orderModel.userId = user.userId;
                orderModel.fName = fName;
                orderModel.lName = lName;
                orderModel.county = county;
                orderModel.address = houseNo + ", " + street + ", " + county;
                orderModel.city = city;
                orderModel.postCode = postCode;
                orderModel.phoneNumber = phoneNumber;
                if(orderNote == null)
                {
                    orderModel.orderNote = "No order note.";

                } else
                {
                    orderModel.orderNote = orderNote;
                }
                orderModel.orderStatus = "0";

                List<OrderItem> orderItems= new List<OrderItem>();
                foreach (CartItemModel item in cartModels)
                {
                    OrderItem orderItem= new OrderItem();
                    orderItem.oItemId = new Guid();
                    orderItem.productId = item.productId;
                    orderItem.qty = item.qty;
                    orderItem.oItemId = new Guid();
                    orderItem.varientOptionId = item.varientOptionId;
                    orderItems.Add(orderItem);
                }
                Debug.WriteLine(orderItems[0].productId);
                orderService.AddOrder(orderModel, orderItems, cartModels[0].cartId);


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

    }
}