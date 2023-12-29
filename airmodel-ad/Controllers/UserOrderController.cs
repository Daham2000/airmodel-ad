using airmodel_ad.Business.Interface;
using airmodel_ad.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace airmodel_ad.Controllers
{
    public class UserOrderController : Controller
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
        string selectedImage = string.Empty;
        Guid VarientId = Guid.Empty;
        Guid productId = Guid.Empty;

        public UserOrderController(IUserService user, IProductService product, ICartService cartService, ICategoryService categoryService, IOrderService orderService)
        {
            this.userService = user;
            this.productService = product;
            this.cartService = cartService;
            this.categoryService = categoryService;
            this.orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
        }

        private async Task<bool> GetHomePageData()
        {
            try
            {
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
                    if (item.varientOptionId != null)
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

                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<IActionResult> PlaceOrder(string fName, string lName, string orderNote, string county, string street, string houseNo, string city, string postCode, string phoneNumber)
        {
            try
            {
                await GetHomePageData();
                string emailValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = userService.GetUserByEmail(emailValue);
                OrderModel orderModel = new OrderModel();
                orderModel.oId = new Guid();
                total = 0;
                int additionalCost = 0;
                foreach (var item in cartModels)
                {
                    total += item.total;
                    additionalCost += item.additionalCost;
                }
                orderModel.total = total;
                orderModel.additionalCost = additionalCost;
                orderModel.userId = user.userId;
                orderModel.fName = fName;
                orderModel.lName = lName;
                orderModel.county = county;
                orderModel.address = houseNo + ", " + street + ", " + county;
                orderModel.city = city;
                orderModel.postCode = postCode;
                orderModel.phoneNumber = phoneNumber;
                orderModel.orderAdminNote = "";
                if (orderNote == null)
                {
                    orderModel.orderNote = "No order note.";

                }
                else
                {
                    orderModel.orderNote = orderNote;
                }
                orderModel.orderStatus = "0";

                List<OrderItem> orderItems = new List<OrderItem>();
                foreach (CartItemModel item in cartModels)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.oItemId = new Guid();
                    orderItem.productId = item.productId;
                    orderItem.qty = item.qty;
                    orderItem.oItemId = new Guid();
                    orderItem.varientOptionId = item.varientOptionId;
                    orderItems.Add(orderItem);
                }
                Debug.WriteLine(orderItems[0].productId);
                orderService.AddOrder(orderModel, orderItems, cartModels[0].cartId);

                var senderEmail = new MailAddress("adcoursework9@gmail.com");
                var receiverEmail = new MailAddress(orderModel.users.userEmail, "Receiver");
                var password = "uifuadhzwaflfqei";

                var sub = "Your order placed Order ID: " + orderModel.oId.ToString().Substring(0, 10);
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password),
                    EnableSsl = true
                };
                using (var mess = new MailMessage("adcoursework9@gmail.com", orderModel.users.userEmail, sub, " <body>\r\n    <h2>Order Status Confirmation</h2>\r\n\r\n    " +
                    "<p>Dear " + orderModel.users.userName + ",</p>\r\n\r\n    <p>Thank you for placing an order with us. Here are the details " +
                    "of your order:</p>\r\n\r\n    <table>\r\n        <tr>\r\n            <td><strong>Order ID: " + orderModel.oId + "</strong></td>\r\n " +
                    "         \r\n        </tr>\r\n        <tr>\r\n            <td><strong>Total Amount:</strong></td>\r\n  " +
                    "          <td>$" + orderModel.total + "</td>\r\n        </tr>\r\n        <!-- Add more details as needed -->\r\n\r\n     " +
                    "   <tr>\r\n            <td><strong>Delivery Address:</strong></td>\r\n            <td>\r\n  " +
                    "              " + orderModel.fName + " " + orderModel.lName + "< br>\r\n                " + orderModel.address + "\r\n" +
                    "                " + orderModel.city + ", " + orderModel.county + "<br>\r\n                " + orderModel.postCode + "\r\n " +
                    "           </td>\r\n        </tr>\r\n    </table>\r\n\r\n  <ul>\r\n      " +

                    "  <p><strong>Order Status: </strong> " + "Pending" + "</p>\r\n    <p><strong>Order Note:</strong> " + orderModel.orderNote + "</p>\r\n\r\n" +
                    "    <p>If you have any questions or concerns about your order, please feel free to contact us.</p>\r\n\r\n   " +
                    " <p>Thank you for choosing our service!</p>\r\n\r\n    <p>Best regards,<br>\r\n    AirModel UK inc.</p>\r\n</body> ")
                {

                })
                {
                    mess.IsBodyHtml = true;
                    smtp.Send(mess);
                }

                total = 0;
                await GetHomePageData();

                ViewBag.productModels = productModels;
                ViewBag.selectedCategory = selectedCategory;
                ViewBag.cartModels = cartModels;
                ViewBag.len = cartModels.Count();
                ViewBag.total = total;
                ViewBag.categories = categories;

                return View("../Home/OrderSuccessView");
            }
            catch (Exception ex)
            {
                return View("../Home/HomeView");
            }
        }
    }
}
