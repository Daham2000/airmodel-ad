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
                    else
                    {
                        pendingOrders[i].orderStatus = "Delivered";
                    }
                }
                ViewBag.orders = null;
                List<string> orderStatusList = new List<string>();
                orderStatusList.Add("Shipped");
                orderStatusList.Add("Delivered");
                ViewBag.orderStatus = orderStatusList;

                List<string> orderStatusFilter = new List<string>();
                orderStatusFilter.Add("Shipped");
                orderStatusFilter.Add("Delivered");
                orderStatusFilter.Add("Pending");
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
                ViewBag.orderStatus = orderStatusList;

                List<string> orderStatusFilter = new List<string>();
                orderStatusFilter.Add("Shipped");
                orderStatusFilter.Add("Delivered");
                orderStatusFilter.Add("Pending");
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
                    else
                    {
                        pendingOrders[i].orderStatus = "Delivered";
                    }
                }
                ViewBag.orders = pendingOrders;
                List<string> orderStatusList = new List<string>();
                orderStatusList.Add("Shipped");
                orderStatusList.Add("Delivered");
                ViewBag.orderStatus = orderStatusList;

                List<string> orderStatusFilter = new List<string>();
                orderStatusFilter.Add("Shipped");
                orderStatusFilter.Add("Delivered");
                orderStatusFilter.Add("Pending");
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
                if (orderStatus == "Shipped")
                {
                    orderStatus = "1";
                } else
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
                    else
                    {
                        pendingOrders[i].orderStatus = "Delivered";
                    }
                }

                ViewBag.orders = pendingOrders;
                List<string> orderStatusList = new List<string>();
                orderStatusList.Add("Shipped");
                orderStatusList.Add("Delivered");
                ViewBag.orderStatus = orderStatusList;

                List<string> orderStatusFilter = new List<string>();
                orderStatusFilter.Add("Shipped");
                orderStatusFilter.Add("Delivered");
                orderStatusFilter.Add("Pending");
                ViewBag.orderStatusFilter = orderStatusFilter;

                OrderModel orderModel = orderService.GetOrderByID(oId);
                var senderEmail = new MailAddress("courseworkt810@gmail.com");
                var receiverEmail = new MailAddress(orderModel.users.userEmail, "Receiver");
                var password = "drmgqctqxnvlagvg";

                var sub = "Your order " + oId.ToString().Substring(0, 10);
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
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }

                return View("../Admin/ManageOrderView");
            }
            catch (Exception ex)
            {
                return View("../Admin/ManageOrderView");
            }
        }
    }
}
