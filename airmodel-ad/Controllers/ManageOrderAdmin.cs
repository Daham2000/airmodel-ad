using airmodel_ad.Business.Interface;
using airmodel_ad.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace airmodel_ad.Controllers
{
    public class ManageOrderAdmin : Controller
    {
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;
        List<OrderModel> orderModels;

        public ManageOrderAdmin(IUserService user, IProductService product, ICartService cartService, ICategoryService categoryService, IOrderService orderService)
        {
            this.userService = user;
            this.productService = product;
            this.cartService = cartService;
            this.categoryService = categoryService;
            this.orderService = orderService;
        }
        public IActionResult Index()
        {
            try
            {
                List<OrderModel> pendingOrders = orderService.GetOrderByStatus("0");
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
                    else if (pendingOrders[i].orderStatus == "3")
                    {
                        pendingOrders[i].orderStatus = "Canceled";
                    }
                    else
                    {
                        pendingOrders[i].orderStatus = "Delivered";
                    }
                }
                ViewBag.orders = pendingOrders;
                List<string> orderStatusList = new List<string>();
                orderStatusList.Add("Shipped");
                orderStatusList.Add("Delivered");
                orderStatusList.Add("Canceled");

                ViewBag.orderStatus = orderStatusList;

                List<string> orderStatusFilter = new List<string>();
                orderStatusFilter.Add("Shipped");
                orderStatusFilter.Add("Delivered");
                orderStatusFilter.Add("Pending");
                orderStatusFilter.Add("Canceled");

                ViewBag.orderStatusFilter = orderStatusFilter;

                return View("../Admin/ManageOrderView");
            }
            catch (Exception ex)
            {
                return View("../Admin/ManageOrderView");
            }
        }

        public IActionResult GetOrderById(Guid oId)
        {
            try
            {
                List<OrderModel> pendingOrders = orderService.GetAllUserOrders();
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
                    else if (pendingOrders[i].orderStatus == "3")
                    {
                        pendingOrders[i].orderStatus = "Canceled";
                    }
                    else
                    {
                        pendingOrders[i].orderStatus = "Delivered";
                    }
                }
                ViewBag.orders = null;
                pendingOrders = pendingOrders.Where((or) => or.oId == oId).ToList();
                ViewBag.fOrders = pendingOrders;
                List<string> orderStatusList = new List<string>();
                orderStatusList.Add("Shipped");
                orderStatusList.Add("Delivered");
                orderStatusList.Add("Canceled");

                ViewBag.orderStatus = orderStatusList;

                List<string> orderStatusFilter = new List<string>();
                orderStatusFilter.Add("Shipped");
                orderStatusFilter.Add("Delivered");
                orderStatusFilter.Add("Pending");
                orderStatusFilter.Add("Canceled");
                ViewBag.orderStatusFilter = orderStatusFilter;

                return View("../Admin/ManageOrderView");
            }
            catch (Exception ex)
            {
                return View("../Admin/ManageOrderView");
            }
        }

        public IActionResult FilterOrderList(string orderStatusSelected)
        {
            try
            {
                string orderStatusSelectedF = orderStatusSelected;
                if (orderStatusSelected == "Pending")
                {
                    orderStatusSelected = "0";
                }
                else if (orderStatusSelected == "Canceled")
                {
                    orderStatusSelected = "3";
                }
                else if (orderStatusSelected == "Shipped")
                {
                    orderStatusSelected = "1";
                }
                else
                {
                    orderStatusSelected = "2";
                }

                List<OrderModel> pendingOrders = orderService.GetOrderByStatus(orderStatusSelected);
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
                    else if (pendingOrders[i].orderStatus == "3")
                    {
                        pendingOrders[i].orderStatus = "Canceled";
                    }
                    else
                    {
                        pendingOrders[i].orderStatus = "Delivered";
                    }
                }
                ViewBag.orders = pendingOrders;
                List<string> orderStatusList = new List<string>();
                orderStatusList.Add("Shipped");
                orderStatusList.Add("Delivered");
                orderStatusList.Add("Canceled");

                ViewBag.orderStatus = orderStatusList;

                List<string> orderStatusFilter = new List<string>();
                orderStatusFilter.Add("Shipped");
                orderStatusFilter.Add("Delivered");
                orderStatusFilter.Add("Pending");
                orderStatusFilter.Add("Canceled");
                ViewBag.orderStatusFilter = orderStatusFilter;
                ViewBag.orderStatusSelected = orderStatusSelectedF;

                return View("../Admin/ManageOrderView");
            }
            catch (Exception ex)
            {
                return View("../Admin/ManageOrderView");
            }
        }

        public IActionResult ChangeStatus(string orderStatus, Guid oId)
        {
            try {
                string o = orderStatus;
                if (orderStatus == "Pending")
                {
                    orderStatus = "0";
                }
                else if (orderStatus == "Canceled")
                {
                    orderStatus = "3";
                }
                else if (orderStatus == "Shipped")
                {
                    orderStatus = "1";
                }
                else
                {
                    orderStatus = "2";
                }
                orderService.ChangeOrderState(oId, orderStatus);
                List<OrderModel> pendingOrders = orderService.GetAllUserOrders();
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
                    else if (pendingOrders[i].orderStatus == "3")
                    {
                        pendingOrders[i].orderStatus = "Canceled";
                    }
                    else
                    {
                        pendingOrders[i].orderStatus = "Delivered";
                    }
                }

                ViewBag.orders = pendingOrders;
                List<string> orderStatusList = new List<string>();
                orderStatusList.Add("Shipped");
                orderStatusList.Add("Delivered");
                orderStatusList.Add("Canceled");

                ViewBag.orderStatus = orderStatusList;

                List<string> orderStatusFilter = new List<string>();
                orderStatusFilter.Add("Shipped");
                orderStatusFilter.Add("Delivered");
                orderStatusFilter.Add("Pending");
                orderStatusFilter.Add("Canceled");
                ViewBag.orderStatusFilter = orderStatusFilter;

                OrderModel orderModel = orderService.GetOrderByID(oId);

                var senderEmail = new MailAddress("adcoursework9@gmail.com");
                var receiverEmail = new MailAddress(orderModel.users.userEmail, "Receiver");
                var password = "uifuadhzwaflfqei";

                var sub = "Order Status Changed - Your order " + oId.ToString().Substring(0, 10);
                var body = "Hi " + orderModel.users.userName + "\nYour order has been moved to " + o + " status...";
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
                    "of your order:</p>\r\n\r\n    <table>\r\n        <tr>\r\n            <td><strong>Order ID: "+ orderModel.oId + "</strong></td>\r\n " +
                    "         \r\n        </tr>\r\n        <tr>\r\n            <td><strong>Total Amount:</strong></td>\r\n  " +
                    "          <td>$"+ orderModel.total + "</td>\r\n        </tr>\r\n        <!-- Add more details as needed -->\r\n\r\n     " +
                    "   <tr>\r\n            <td><strong>Delivery Address:</strong></td>\r\n            <td>\r\n  " +
                    "              " + orderModel.fName + " " + orderModel.lName + "< br>\r\n                "+ orderModel.address + "<br>\r\n" +
                    "                " + orderModel.city + ", " + orderModel.county + "<br>\r\n                " + orderModel.postCode + "\r\n " +
                    "           </td>\r\n        </tr>\r\n    </table>\r\n\r\n  <ul>\r\n      " +

                    "  <p><strong>Order Status:</strong> " + o + "</p>\r\n    <p><strong>Order Note:</strong> " + orderModel.orderNote + "</p>\r\n\r\n" +
                    "    <p>If you have any questions or concerns about your order, please feel free to contact us.</p>\r\n\r\n   " +
                    " <p>Thank you for choosing our service!</p>\r\n\r\n    <p>Best regards,<br>\r\n    AirModel UK inc.</p>\r\n</body> ")
                {
                    
                })
                {
                    mess.IsBodyHtml = true;
                    smtp.Send(mess);
                }

                return View("../Admin/ManageOrderView");
            }
            catch (Exception ex)
            {
                return View("../Admin/ManageOrderView");
            }
        }

        public IActionResult AddOrderNote(string orderAdminNote, Guid oId)
        {
            List<OrderModel> pendingOrders = orderService.GetAllUserOrders();
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
                else if (pendingOrders[i].orderStatus == "3")
                {
                    pendingOrders[i].orderStatus = "Canceled";
                }
                else
                {
                    pendingOrders[i].orderStatus = "Delivered";
                }
            }

            ViewBag.orders = pendingOrders;
            List<string> orderStatusList = new List<string>();
            orderStatusList.Add("Shipped");
            orderStatusList.Add("Delivered");
            orderStatusList.Add("Canceled");

            ViewBag.orderStatus = orderStatusList;

            List<string> orderStatusFilter = new List<string>();
            orderStatusFilter.Add("Shipped");
            orderStatusFilter.Add("Delivered");
            orderStatusFilter.Add("Pending");
            orderStatusFilter.Add("Canceled");
            ViewBag.orderStatusFilter = orderStatusFilter;

            OrderModel orderModel = orderService.GetOrderByID(oId);

            var senderEmail = new MailAddress("adcoursework9@gmail.com");
            var receiverEmail = new MailAddress(orderModel.users.userEmail, "Receiver");
            var password = "uifuadhzwaflfqei";

            var sub = "You have a message from the Seller - Your order " + oId.ToString().Substring(0, 10);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password),
                EnableSsl = true
            };
            using (var mess = new MailMessage("adcoursework9@gmail.com", orderModel.users.userEmail, sub, " <body>\r\n    <h2>You have a message from the Seller.</h2>\r\n\r\n    " +
                "<p>Dear " + orderModel.users.userName + ",</p>\r\n\r\n    <p>Thank you for placing an order with us. Here is the message from the Seller " +
                "\r\n\r\n" + "Message:  " + orderAdminNote + "\n" +
                "<p>If you have any questions or concerns about your order, please feel free to contact us.</p>\r\n\r\n   " +
                " <p>Thank you for choosing our service!</p>\r\n\r\n    <p>Best regards,<br>\r\n    AirModel UK inc.</p>\r\n</body> ")
            {
            })
            {
                mess.IsBodyHtml = true;
                smtp.Send(mess);
            }

            OrderModel orderModel1 = orderService.GetOrderByID(oId);


            return View("../Admin/ManageOrderView");
        }
      }
}
