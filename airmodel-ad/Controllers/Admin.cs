﻿using airmodel_ad.Business.Interface;
using airmodel_ad.Models;
using airmodel_ad.Models.ParamModels;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace airmodel_ad.Controllers
{
    public class Admin : Controller
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

        public Admin(IUserService user, IProductService product, ICartService cartService, ICategoryService categoryService, IOrderService orderService)
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

        public IActionResult GenerateOrderBasicReportByStatus(string orderStatusSelected)
        {
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
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new PdfWriter(stream))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);

                            if (pendingOrders.Count != 0)
                            {
                                //Create report title and subtitles
                                document.Add(new Paragraph("Orders Report").SetTextAlignment(TextAlignment.CENTER).SetFontSize(12));
                                document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));

                                //Create report table
                                var table = new Table(new float[] { 1, 1, 1, 1, 1, 1 }).SetFontSize(10);
                                table.AddHeaderCell(new Cell().Add(new Paragraph("#")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Order ID")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Customer Name")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Order Total")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Order Phonenumber")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Order Status")));

                                for (int i = 0; i < pendingOrders.Count; i++)
                                {
                                    table.AddCell(new Cell().Add(new Paragraph((i + 1).ToString())));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].oId.ToString())));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].fName + " " + pendingOrders[i].lName)));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].total.ToString())));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].phoneNumber.ToString())));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].orderStatus.ToString())));
                                }
                                document.Add(table);
                            }
                            else
                            {
                                document.Add(new Paragraph("No orders data!").SetTextAlignment(TextAlignment.CENTER).SetFontSize(12));
                            }

                            document.Close();
                        }
                    }
                    return File(stream.ToArray(), "application/pdf", "Order Report.pdf");
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.orders = pendingOrders;

            return View("../Admin/AdminView");
        }

        public IActionResult GenerateOrderBasicReportByID(Guid id)
        {
            List<OrderModel> pendingOrders = orderService.GetAllUserOrders();
            OrderModel orderModel = pendingOrders.Where((or) => or.oId == id).FirstOrDefault();
            User user = userService.GetUserByUid(orderModel.userId);

            orderModel.orderItems = orderService.GetAllOrderItems(id);
            if (orderModel.orderStatus == "0")
            {
                orderModel.orderStatus = "Pending";
            }
            else if (orderModel.orderStatus == "1")
            {
                orderModel.orderStatus = "Shipped";
            }
            else
            {
                orderModel.orderStatus = "Delivered";
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new PdfWriter(stream))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);
                            //Create report title and subtitles
                            document.Add(new Paragraph("Orders Report").SetTextAlignment(TextAlignment.CENTER).SetFontSize(13));
                            document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy")).SetTextAlignment(TextAlignment.LEFT).SetFontSize(10));
                            document.Add(new Paragraph("Order ID: " + orderModel.oId).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("Customer Name: " + orderModel.fName + " " + orderModel.lName).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("Order Total: " + orderModel.total).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("Order Phonenumber: " + orderModel.phoneNumber).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("Order Status: " + orderModel.orderStatus).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("Order Note: " + orderModel.orderNote).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("--------------------------------------").SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("User Account ID: " + user.userId).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("User Account Email: " + user.userEmail).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));
                            document.Add(new Paragraph("User Account's User Name: " + user.userName).SetTextAlignment(TextAlignment.LEFT).SetFontSize(11));

                            document.Close();
                        }
                    }
                    return File(stream.ToArray(), "application/pdf", "Order Report.pdf");
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.orders = pendingOrders;

            return View("../Admin/AdminView");
        }

        public IActionResult GenerateOrderBasicReport()
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
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new PdfWriter(stream))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);

                            if (pendingOrders.Count != 0)
                            {
                                //Create report title and subtitles
                                document.Add(new Paragraph("Orders Report").SetTextAlignment(TextAlignment.CENTER).SetFontSize(12));
                                document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));

                                //Create report table
                                var table = new Table(new float[] { 1, 1, 1, 1, 1, 1 }).SetFontSize(10);
                                table.AddHeaderCell(new Cell().Add(new Paragraph("#")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Order ID")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Customer Name")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Order Total")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Order Phonenumber")));
                                table.AddHeaderCell(new Cell().Add(new Paragraph("Order Status")));

                                for (int i = 0; i < pendingOrders.Count; i++)
                                {
                                    table.AddCell(new Cell().Add(new Paragraph((i + 1).ToString())));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].oId.ToString())));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].fName + " " + pendingOrders[i].lName)));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].total.ToString())));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].phoneNumber.ToString())));
                                    table.AddCell(new Cell().Add(new Paragraph(pendingOrders[i].orderStatus.ToString())));
                                }
                                document.Add(table);
                            }
                            else
                            {
                                document.Add(new Paragraph("No orders data!").SetTextAlignment(TextAlignment.CENTER).SetFontSize(12));
                            }

                            document.Close();
                        }
                    }
                    return File(stream.ToArray(), "application/pdf", "Order Report.pdf");
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.orders = pendingOrders;

            return View("../Admin/AdminView");
        }

        public IActionResult GetAllUsers()
        {
            List<User> users = userService.GetAllUsers();
            ViewBag.users = users;
            ViewBag.isUserEdit = false;
            List<string> userRoles = new List<string>();
            userRoles.Add("user");
            userRoles.Add("admin");
            ViewBag.userRoles = userRoles;
            ViewBag.message = "Added";

            return View("../Admin/ManageUsers");
        }

        public IActionResult OpenAddUserView()
        {
            ViewBag.isUserEdit = false;
            ViewBag.isAddUser = true;
            List<string> userRoles = new List<string>();
            userRoles.Add("user");
            userRoles.Add("admin");
            ViewBag.userRoles = userRoles;
            return View("../Admin/ManageUsers");
        }

        public IActionResult DeleteUser(Guid userId)
        {
            userService.DeleteUser(userId);

            List<User> users = userService.GetAllUsers();
            ViewBag.users = users;
            ViewBag.isUserEdit = false;
            List<string> userRoles = new List<string>();
            userRoles.Add("user");
            userRoles.Add("admin");
            ViewBag.userRoles = userRoles;

            return View("../Admin/ManageUsers");
        }

        public IActionResult AddUserAction(string userName, string userEmail, string userRole, string userPassword)
        {
            SignupModel signupModel = new SignupModel();
            signupModel.UserName = userName;
            signupModel.UserEmail = userEmail;
            signupModel.UserPassword = userPassword;
            userService.AddUser(signupModel);

            List<User> users = userService.GetAllUsers();
            ViewBag.users = users;
            ViewBag.isUserEdit = false;
            List<string> userRoles = new List<string>();
            userRoles.Add("user");
            userRoles.Add("admin");
            ViewBag.userRoles = userRoles;

			return View("../Admin/ManageUsers");
        }

        public IActionResult DownloadUserReport()
        {
            List<User> users = userService.GetAllUsers();
            ViewBag.users = users;
            ViewBag.isUserEdit = false;

            using (var stream = new MemoryStream())
            {
                using (var writer = new PdfWriter(stream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        if (users.Count != 0)
                        {
                            //Create report title and subtitles
                            document.Add(new Paragraph("All Users Report").SetTextAlignment(TextAlignment.CENTER).SetFontSize(12));
                            document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));

                            //Create report table
                            var table = new Table(new float[] { 1, 1, 1, 1, 1 }).SetFontSize(8);
                            table.AddHeaderCell(new Cell().Add(new Paragraph("#")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("User ID")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("User Name")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("User Email")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("User Role")));

                            for (int i = 0; i < users.Count; i++)
                            {
                                table.AddCell(new Cell().Add(new Paragraph((i + 1).ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(users[i].userId.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(users[i].userName)));
                                table.AddCell(new Cell().Add(new Paragraph(users[i].userEmail.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(users[i].userRole.ToString())));
                            }
                            document.Add(table);
                        }
                        else
                        {
                            document.Add(new Paragraph("No user data!").SetTextAlignment(TextAlignment.CENTER).SetFontSize(9));
                        }

                        document.Close();
                    }
                }
                return File(stream.ToArray(), "application/pdf", "All Users Report.pdf");
            }

            return View("../Admin/ManageUsers");
        }

        public IActionResult EditUserAction(Guid userId, string userName, string userEmail, string userRole)
        {
            User user = userService.GetUserByUid(userId);
            user.userName = userName;
            user.userEmail = userEmail;
            user.userRole = userRole;
            userService.EditUser(user.userId, user);
            ViewBag.isUserEdit = true;
            user = userService.GetUserByUid(userId);
            ViewBag.user = user;
            List<string> userRoles = new List<string>();
            userRoles.Add("user");
            userRoles.Add("admin");
            ViewBag.userRoles = userRoles;


            List<OrderModel> ordersList = orderService.GetAllOrders(userId);
            for (int i = 0; i <= ordersList.Count() - 1; i++)
            {
                ordersList[i].orderItems = orderService.GetAllOrderItems(ordersList[i].oId);
                if (ordersList[i].orderStatus == "0")
                {
                    ordersList[i].orderStatus = "Pending";
                }
                else if (ordersList[i].orderStatus == "1")
                {
                    ordersList[i].orderStatus = "Shipped";
                }
                else
                {
                    ordersList[i].orderStatus = "Delivered";
                }
            }
            ViewBag.fOrders = ordersList;

            List<string> orderStatusList = new List<string>();
            orderStatusList.Add("Shipped");
            orderStatusList.Add("Delivered");
            ViewBag.orderStatus = orderStatusList;
			ViewBag.isUserEdit = false;
			List<User> users = userService.GetAllUsers();
			ViewBag.users = users;

			return View("../Admin/ManageUsers");
        }

        public IActionResult EditUserView(Guid userId)
        {
            User user = userService.GetUserByUid(userId);
            ViewBag.isUserEdit = true;
            ViewBag.user = user;
            List<string> userRoles = new List<string>();
            userRoles.Add("user");
            userRoles.Add("admin");
            ViewBag.userRoles = userRoles;

            List<OrderModel> ordersList = orderService.GetAllOrders(userId);
            for (int i = 0; i <= ordersList.Count() - 1; i++)
            {
                ordersList[i].orderItems = orderService.GetAllOrderItems(ordersList[i].oId);
                if (ordersList[i].orderStatus == "0")
                {
                    ordersList[i].orderStatus = "Pending";
                }
                else if (ordersList[i].orderStatus == "1")
                {
                    ordersList[i].orderStatus = "Shipped";
                }
                else
                {
                    ordersList[i].orderStatus = "Delivered";
                }
            }
            ViewBag.fOrders = ordersList;
            List<string> orderStatusList = new List<string>();
            orderStatusList.Add("Shipped");
            orderStatusList.Add("Delivered");
            ViewBag.orderStatus = orderStatusList;


            return View("../Admin/ManageUsers");
        }

        public IActionResult OpenManageCategories()
        {
            List<Category> categories1 = categoryService.GetAllCategories();
            ViewBag.categories1 = categories1;
            ViewBag.editCategory = false;
            ViewBag.addCategory = false;
            return View("../Admin/ManageCategoriesView");
        }

        public IActionResult OpenAddCategories()
        {
            List<Category> categories1 = categoryService.GetAllCategories();
            ViewBag.categories1 = categories1;
            ViewBag.editCategory = false;
            ViewBag.addCategory = true;
            return View("../Admin/ManageCategoriesView");
        }

        public IActionResult OpenEditCategories(Guid categoryId, string categoryName)
        {
            Category category = new Category();
            category.categoryId = categoryId;
            category.categoryName = categoryName;
            ViewBag.category = category;

            List<Category> categories1 = categoryService.GetAllCategories();
            ViewBag.categories1 = categories1;
            ViewBag.editCategory = true;
            ViewBag.addCategory = false;
            return View("../Admin/ManageCategoriesView");
        }

        public IActionResult EditCategoriesAction(Guid categoryId, string categoryName)
        {
            Debug.WriteLine("categoryId");
            Debug.WriteLine(categoryId);
            categoryService.EditCategory(categoryName, categoryId);
            List<Category> categories1 = categoryService.GetAllCategories();
            ViewBag.categories1 = categories1;
            ViewBag.editCategory = false;
            ViewBag.addCategory = false;
            return View("../Admin/ManageCategoriesView");
        }

        public IActionResult AddCategoriesAction(string categoryName)
        {
            Category category = new Category();
            category.categoryId = new Guid();
            category.categoryName = categoryName;
            categoryService.AddCategory(category);
            List<Category> categories1 = categoryService.GetAllCategories();
            ViewBag.categories1 = categories1;
            ViewBag.editCategory = false;
            ViewBag.addCategory = false;

            return View("../Admin/ManageCategoriesView");
        }
    }

}
