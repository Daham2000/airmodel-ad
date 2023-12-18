using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using System.Diagnostics;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace airmodel_ad.Business.Services
{
    public class ProductService : IProductService
    {
        AppDbContext appDbContext;
        public ProductService(AppDbContext context) { 
            appDbContext = context;
        }

        public List<ProductModel> GetAllAvailableProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            try
            {
                products = appDbContext.products.Where((p) => p.productQty > 0).ToList();
                return products;
            }
            catch (Exception ex)
            {
                return products;
            }
        }

        public List<ProductModel> GetAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            try {
                List<Category> categories = appDbContext.category.ToList();

                products = appDbContext.products.ToList();
                for (int i = 0; i <= products.Count() - 1; i++)
                {
                    products[i].categoryName = categories.Where((ca) => ca.categoryId == products[i].categoryId).FirstOrDefault().categoryName;
                }
                return products;
            } catch (Exception ex) {
                return products;
            }
        }

        public List<ProductModel> GetAllProductsByCategory(Guid categoryId)
        {
            List<ProductModel> products = new List<ProductModel>();
            try
            {
                List<Category> categories = appDbContext.category.ToList();
                products = appDbContext.products.Where((p) => p.categoryId == categoryId).ToList();
                for(int i=0; i<=products.Count()-1; i++)
                {
                    products[i].categoryName = categories.Where((ca) => ca.categoryId == products[i].categoryId).FirstOrDefault().categoryName;
                }
                return products;
            }
            catch (Exception ex)
            {
                return products;
            }
        }

        public List<ProductModel> GetSearchProduct(string searchInput)
        {
            List<ProductModel> products = new List<ProductModel>();
            try
            {
                products = appDbContext.products.Where(u => u.productName.ToLower().Contains(searchInput.ToLower())).ToList();
                return products;
            }
            catch (Exception ex)
            {
                return products;
            }
        }

        public ProductModel GetProductById(Guid searchInput)
        {
            ProductModel product = new ProductModel();
            try
            {
                product = appDbContext.products.Where(u => u.productId == searchInput).FirstOrDefault();
                VarientModel? varientModel = appDbContext.varient.Where(u => u.productId == searchInput).FirstOrDefault();
                List<VarientOptionModel> varientOptionModels = appDbContext.varientOption.Where(u => u.varientId == varientModel.varientId).ToList();
                List<VarientModel> varientModels = appDbContext.varient.Where(u => u.productId == product.productId).ToList();
                product.categoryName = appDbContext.category.Where((ca) => ca.categoryId == product.categoryId).FirstOrDefault().categoryName;

                product.varientOptionModels = varientOptionModels;
                product.varientModels = varientModels;
                return product;
            }
            catch (Exception ex)
            {
                return product;
            }
        }

        bool IProductService.AddProduct(ProductModel product)
        {
            try { 
                appDbContext.Add(product);
                appDbContext.SaveChanges();
                VarientModel colorVarient = new VarientModel();
                colorVarient.productId = product.productId;
                colorVarient.varientId = new Guid();
                colorVarient.productName = "Color";
                VarientModel sizeVarient = new VarientModel();
                colorVarient.productId = product.productId;
                colorVarient.varientId = new Guid();
                colorVarient.productName = "Size";
                appDbContext.Add(colorVarient);
                appDbContext.Add(sizeVarient);
                appDbContext.SaveChanges();
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        bool IProductService.AddProductVarient(VarientModel varientModel)
        {
            try
            {
                appDbContext.Add(varientModel);
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        bool IProductService.AddProductVarientItem(VarientOptionModel varientOption)
        {
            try
            {
                appDbContext.Add(varientOption);
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public VarientOptionModel GetProductVarientById(Guid searchInput)
        {
            VarientOptionModel varientOptionModels = new VarientOptionModel();
            try
            {
                varientOptionModels = appDbContext.varientOption.Where(u => u.varientOptionId == searchInput).FirstOrDefault();

                return varientOptionModels;
            }
            catch (Exception ex)
            {
                return varientOptionModels;
            }
        }

        public bool EditProduct(ProductModel product)
        {
            try
            {
                appDbContext.SaveChanges();
                if(product.varientOptionModels != null)
                {
                    foreach (var varientOption in product.varientOptionModels)
                    {
                        VarientOptionModel? varientOptionModel = appDbContext.varientOption.Where((v) => v.varientId == varientOption.varientId).FirstOrDefault();
                        varientOptionModel = varientOption;
                        appDbContext.SaveChanges();
                    }
                }

                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
