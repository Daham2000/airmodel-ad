using airmodel_ad.Business.Interface;
using airmodel_ad.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;

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
                ViewBag.orders = pendingOrders;
                List<string> orderStatusList = new List<string>();
                orderStatusList.Add("Shipped");
                orderStatusList.Add("Delivered");
                ViewBag.orderStatus = orderStatusList;
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
                if(orderStatus == "Shipped")
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
                return View("../Admin/ManageOrderView");
            }
            catch (Exception ex)
            {
                return View("../Admin/ManageOrderView");
            }
        }
    }
}
