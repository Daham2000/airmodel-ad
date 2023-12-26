using airmodel_ad.Models;

namespace airmodel_ad.Business.Interface
{
    public interface ICartService
    {
        public CartModel GetCart(string id);
        public List<CartItemModel> GetCartItem(Guid id);
        public bool AddCart(CartItemModel item);
        public bool RemoveCartItem(CartItemModel item);
        public bool CheckProductAvailableInCart(ProductModel item, Guid userId);
    }
}
