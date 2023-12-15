using airmodel_ad.Models;

namespace airmodel_ad.Business.Interface
{
    public interface IProductService
    {
        public List<ProductModel> GetAllProducts();
        public List<ProductModel> GetAllProductsByCategory(Guid categoryId);
        public List<ProductModel> GetSearchProduct(string searchInput);
        public ProductModel GetProductById(Guid searchInput);
        public VarientOptionModel GetProductVarientById(Guid searchInput);
        public List<ProductModel> GetAllAvailableProducts();
        public bool AddProduct(ProductModel product);
        public bool AddProductVarient(VarientModel varientModel);
        public bool AddProductVarientItem(VarientOptionModel varientOption);
    }
}
