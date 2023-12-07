using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using System.Diagnostics;

namespace airmodel_ad.Business.Services
{
    public class CartService : ICartService
    {
        AppDbContext appDbContext;
        public CartService(AppDbContext context)
        {
            appDbContext = context;
        }

        public bool AddCart(CartItemModel item)
        {
            try
            {
                appDbContext.Add(item);
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public CartModel GetCart(string name)
        {
            CartModel cartModel = new CartModel();
            try {
                User user = appDbContext.users.Where((u) => u.userEmail == name).FirstOrDefault();
                Debug.WriteLine("user: ", user.userId);

                cartModel = appDbContext.carts.Where((u) => u.userId == user.userId).FirstOrDefault();
                return cartModel;
            } catch(Exception ex)
            {
                return cartModel;
            }
        }

        public List<CartItemModel> GetCartItem(Guid id)
        {
            List<CartItemModel> cartModel = new List<CartItemModel>();
            try
            {
                cartModel = appDbContext.cartItems.Where((u) => u.cartId == id).ToList();
                for (int i=0; i < cartModel.Count(); i+=1)
                {
                    cartModel[i].varientOption = appDbContext.varientOption.Where((v) => v.varientOptionId == cartModel[i].varientOptionId).FirstOrDefault();
                }
                return cartModel;
            }
            catch (Exception ex)
            {
                return cartModel;
            }
        }

        public bool RemoveCartItem(CartItemModel item)
        {
            try {
                appDbContext.cartItems.Remove(item);
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}
