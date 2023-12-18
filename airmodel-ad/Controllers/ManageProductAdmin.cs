using airmodel_ad.Business.Interface;
using airmodel_ad.Business.Services;
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
                ProductModel item = productService.GetProductById(productId);
                List<Category> categories = categoryService.GetAllCategories();
                ViewBag.allItemsList = allItemsList;
                ViewBag.item = item;
                ViewBag.categories = categories;
                return View("../Admin/EditSingleProductAdmin");
            }
            catch (Exception ex)
            {
                return View("../Admin/EditSingleProductAdmin");
            }
        }

        public IActionResult EditProductOnSubmit(Guid productId, string productName, string categoryName, int productBasicPrice, string productImage, int productQty)
        {
            try
            {
                List<Category> categories = categoryService.GetAllCategories();

                ProductModel productModel = productService.GetProductById(productId);
                productModel.productName = productName;
                productModel.categoryId = categories.Where((ca) => ca.categoryName == categoryName).FirstOrDefault().categoryId;
                productModel.productBasicPrice = productBasicPrice;
                productModel.productImage = productImage;
                productModel.productQty = productQty;
                productService.EditProduct(productModel);

                List<ProductModel> allItemsList = productService.GetAllProducts();
                ProductModel item = productService.GetProductById(productId);
                ViewBag.allItemsList = allItemsList;
                ViewBag.item = item;
                ViewBag.categories = categories;
                return View("../Admin/EditSingleProductAdmin");
            }
            catch (Exception ex)
            {
                return View("../Admin/EditSingleProductAdmin");
            }
        }
    }
}
