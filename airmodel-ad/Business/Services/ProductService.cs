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
                products = appDbContext.products.ToList();
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
                products = appDbContext.products.Where((p) => p.categoryId == categoryId).ToList();
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
    }
}
