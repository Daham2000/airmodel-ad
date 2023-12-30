using airmodel_ad.Business.Interface;
using airmodel_ad.Business.Services;
using airmodel_ad.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
                item.categoryName = categories.Where((ca) => ca.categoryId == item.categoryId).FirstOrDefault().categoryName;
                ViewBag.allItemsList = allItemsList;
                ViewBag.item = item;
                ViewBag.categories = categories;
                List<string> booll = new List<string>();
                booll.Add("Yes");
                booll.Add("No");
                ViewBag.booll = booll;
                ViewBag.hasVarients = item.hasVarients == true ? "Yes" : "No";

                return View("../Admin/EditSingleProductAdmin");
            }
            catch (Exception ex)
            {
                return View("../Admin/EditSingleProductAdmin");
            }
        }

        public IActionResult EditProductOnSubmit(Guid productId, string hasVarients, string productName, string categoryName, int productBasicPrice, string productImage, int productQty)
        {
            try
            {
                List<Category> categories = categoryService.GetAllCategories();
                ProductModel productModel = productService.GetProductById(productId);
                productModel.productName = productName;
                productModel.categoryId = categories.Where((ca) => ca.categoryName == categoryName).FirstOrDefault().categoryId;
                productModel.productBasicPrice = productBasicPrice;
                Debug.WriteLine("hasVarients");
                Debug.WriteLine(hasVarients);
                if (hasVarients == "Yes")
                {
                    productModel.hasVarients = true;
                } else
                {
                    productModel.hasVarients = false;
                }
                productModel.productImage = productImage;
                productModel.productQty = productQty;
                productService.EditProduct(productModel);

                List<ProductModel> allItemsList = productService.GetAllProducts();
                ProductModel item = productService.GetProductById(productId);
                item.categoryName = categories.Where((ca) => ca.categoryId == item.categoryId).FirstOrDefault().categoryName;
                ViewBag.allItemsList = allItemsList;
                ViewBag.item = item;
                ViewBag.categories = categories;
                List<string> booll = new List<string>();
                booll.Add("Yes");
                booll.Add("No");
                ViewBag.booll = booll;
                ViewBag.hasVarients = item.hasVarients == true ? "Yes" : "No";

                return View("../Admin/EditSingleProductAdmin");
            }
            catch (Exception ex)
            {
                return View("../Admin/EditSingleProductAdmin");
            }
        }

        public IActionResult EditVarientOnSubmit(Guid productId, Guid varientOptionId, string varientName, int varientPrice, int varientQty, string varientImage)
        {
            try
            {
                List<Category> categories = categoryService.GetAllCategories();

                VarientOptionModel varientOptionModel = productService.GetProductVarientById(varientOptionId);
                varientOptionModel.varientName = varientName;
                varientOptionModel.varientPrice = varientPrice;
                varientOptionModel.varientQty = varientQty;
                varientOptionModel.varientImage = varientImage;

                productService.EditProductVarientItem(varientOptionModel);

                List<ProductModel> allItemsList = productService.GetAllProducts();
                ProductModel item = productService.GetProductById(productId);
                item.categoryName = categories.Where((ca) => ca.categoryId == item.categoryId).FirstOrDefault().categoryName;
                ViewBag.allItemsList = allItemsList;
                ViewBag.item = item;
                ViewBag.categories = categories;
                List<string> booll = new List<string>();
                booll.Add("Yes");
                booll.Add("No");
                ViewBag.booll = booll;
                ViewBag.hasVarients = item.hasVarients == true ? "Yes" : "No";

                return View("../Admin/EditSingleProductAdmin");
            }
            catch (Exception ex)
            {
                return View("../Admin/EditSingleProductAdmin");
            }
        }

        public IActionResult AddVarientOnSubmit(Guid productId, Guid varientId, string varientName, int varientPrice, int varientQty, string varientImage)
        {
            try
            {
                List<Category> categories = categoryService.GetAllCategories();

                VarientOptionModel varientOptionModel = new VarientOptionModel();
                varientOptionModel.varientId = varientId;
                varientOptionModel.varientOptionId = new Guid();
                varientOptionModel.varientName = varientName;
                varientOptionModel.varientPrice = varientPrice;
                varientOptionModel.varientQty = varientQty;
                varientOptionModel.varientImage = varientImage;

                productService.AddProductVarientItem(varientOptionModel);

                List<ProductModel> allItemsList = productService.GetAllProducts();
                ProductModel item = productService.GetProductById(productId);
                item.categoryName = categories.Where((ca) => ca.categoryId == item.categoryId).FirstOrDefault().categoryName;
                ViewBag.allItemsList = allItemsList;
                ViewBag.item = item;
                ViewBag.categories = categories;
                List<string> booll = new List<string>();
                booll.Add("Yes");
                booll.Add("No");
                ViewBag.booll = booll;
                ViewBag.hasVarients = item.hasVarients == true ? "Yes" : "No";

                return View("../Admin/EditSingleProductAdmin");
            }
            catch (Exception ex)
            {
                return View("../Admin/EditSingleProductAdmin");
            }
        }

        public IActionResult DeleteVariationOption(Guid productId, Guid varientOptionId)
        {
            List<Category> categories = categoryService.GetAllCategories();

            productService.DeleteProductVarientOption(varientOptionId);

            List<ProductModel> allItemsList = productService.GetAllProducts();
            ProductModel item = productService.GetProductById(productId);
            item.categoryName = categories.Where((ca) => ca.categoryId == item.categoryId).FirstOrDefault().categoryName;
            ViewBag.allItemsList = allItemsList;
            ViewBag.item = item;
            ViewBag.categories = categories;
            List<string> booll = new List<string>();
            booll.Add("Yes");
            booll.Add("No");
            ViewBag.booll = booll;
            ViewBag.hasVarients = item.hasVarients == true ? "Yes" : "No";

            return View("../Admin/EditSingleProductAdmin");
        }
        public IActionResult AddProduct()
        {
            try
            {
                List<Category> categories = categoryService.GetAllCategories();
                ViewBag.categories = categories;

                ProductModel productModel = new ProductModel();
                productModel.productId = new Guid();
                Debug.WriteLine("productModel.productId ");
                Debug.WriteLine(productModel.productId);
                productModel.productName = "";
                productModel.productDescription = "";
                productModel.productImage = "";
                productModel.productBasicPrice = 0;
                productModel.productQty = 0;
                productModel.hasVarients = false;
                productModel.categoryId = categories[0].categoryId;
                productModel.categoryName = categories[0].categoryName;
                ViewBag.item = productModel;
                List<string> booll = new List<string>();
                booll.Add("Yes");
                booll.Add("No");
                ViewBag.booll = booll;
                ViewBag.hasVarients = "No";

                return View("../Admin/AddProductView");
            }
            catch (Exception ex)
            {
                return View("../Admin/AddProductView");
            }
        }

        public IActionResult AddProductOnSubmit(Guid productId, string productDescription, string hasVarients, string productName, string categoryName, int productBasicPrice, string productImage, int productQty)
        {
            try
            {
                List<Category> categories = categoryService.GetAllCategories();
                ViewBag.categories = categories;

                ProductModel productModel = new ProductModel();
                productModel.productName = productName;
                productModel.productDescription = productDescription;
                productModel.categoryId = categories.Where((ca) => ca.categoryName == categoryName).FirstOrDefault().categoryId;
                productModel.productBasicPrice = productBasicPrice;
                Debug.WriteLine("productName");
                Debug.WriteLine(productName);
                Debug.WriteLine(productDescription);
                Debug.WriteLine(productId);
                if (hasVarients == "Yes")
                {
                    productModel.hasVarients = true;
                }
                else
                {
                    productModel.hasVarients = false;
                }
                productModel.productImage = productImage;
                productModel.productQty = productQty;
                productService.AddProduct(productModel);

                List<ProductModel> allItemsList = productService.GetAllProducts();
                ProductModel item = allItemsList.Where((p) => p.productName == productName).FirstOrDefault();

                item.categoryName = categoryName;
                ViewBag.allItemsList = allItemsList;
                ViewBag.item = item;
                ViewBag.categories = categories;
                List<string> booll = new List<string>();
                booll.Add("Yes");
                booll.Add("No");
                ViewBag.booll = booll;
                ViewBag.hasVarients = item.hasVarients == true ? "Yes" : "No";

				return Index();

			}
			catch (Exception ex)
            {
                return View("../Admin/AddProductView");
            }
        }
    }
}
