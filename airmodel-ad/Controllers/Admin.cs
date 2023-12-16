using airmodel_ad.Business.Interface;
using airmodel_ad.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;

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
            } catch (Exception ex)
            {

            }
            ViewBag.orders = pendingOrders;

            return View("../Admin/AdminView");
        }
    }
}
