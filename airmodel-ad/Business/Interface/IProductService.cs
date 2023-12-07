using airmodel_ad.Models;

namespace airmodel_ad.Business.Interface
{
    public interface IProductService
    {
        public List<ProductModel> GetAllProducts();
        public List<ProductModel> GetAllAvailableProducts();
        public bool AddProduct(ProductModel product);
        public bool AddProductVarient(VarientModel varientModel);
        public bool AddProductVarientItem(VarientOptionModel varientOption);
    }
}
