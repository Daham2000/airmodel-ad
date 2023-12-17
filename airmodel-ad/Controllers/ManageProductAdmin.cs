using airmodel_ad.Business.Interface;
using airmodel_ad.Models;
using Microsoft.AspNetCore.Mvc;

namespace airmodel_ad.Controllers
{
    public class ManageProductAdmin : Controller
    {
        List<ProductModel> productModels;

        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;

        public ManageProductAdmin(IUserService user, IProductService product, ICartService cartService, ICategoryService categoryService, IOrderService orderService)
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
                List<ProductModel> allItemsList = productService.GetAllProducts();
                List<Category> categories = categoryService.GetAllCategories();
                ViewBag.allItemsList = allItemsList;
                ViewBag.categories = categories;
                return View("../Admin/ManageProductsView");
            }
            catch (Exception ex)
            {
                return View("../Admin/ManageProductsView");
            }
        }

        public IActionResult EditProduct(Guid productId)
        {
            try
            {
                List<ProductModel> allItemsList = productService.GetAllProducts();
                List<Category> categories = categoryService.GetAllCategories();
                ViewBag.allItemsList = allItemsList;
                ViewBag.categories = categories;
                return View("../Admin/ManageProductsView");
            }
            catch (Exception ex)
            {
                return View("../Admin/ManageProductsView");
            }
        }
    }
}
