using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlTypes;
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
                appDbContext.cartItems.Add(item);
                if (item.varientOptionId != null)
                {
                    VarientOptionModel? varientOptionModel = appDbContext.varientOption.Where((o) => o.varientOptionId == item.varientOptionId).FirstOrDefault();
                }
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckProductAvailableInCart(ProductModel item, Guid userId)
        {
            try {
                CartModel? cartModel = appDbContext.carts.Where((u) => u.userId == userId).FirstOrDefault();
                CartItemModel cartItemModel = appDbContext.cartItems.Where((p) => p.productId == item.productId).Where((u) => u.cartId == cartModel.cartId).FirstOrDefault();
                Debug.WriteLine(cartItemModel); 
                if(cartItemModel != null)
                {
                    return true;
                }
                return false;
            }catch(Exception ex) { 
                return false;
            }
        }

        public CartModel GetCart(string name)
        {
            CartModel cartModel = new CartModel();
            try {
                User user = appDbContext.users.Where((u) => u.userEmail == name).FirstOrDefault();

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
                    if(cartModel[i].varientOptionId != null)
                    {
                        if (cartModel[i].varientOptionId.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            cartModel[i].varientOption = appDbContext.varientOption.Where((v) => v.varientOptionId == cartModel[i].varientOptionId).FirstOrDefault();
                            cartModel[i].varientOption.varientImage = appDbContext.varientOption.Where((v) => v.varientOptionId == cartModel[i].varientOptionId).FirstOrDefault().varientImage;
                        }
                        else
                        {
                            cartModel[i].varientOption = appDbContext.varientOption.Where((v) => v.varientOptionId == cartModel[i].varientOptionId).FirstOrDefault();
                        }
                    }
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
                appDbContext.SaveChanges();
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}
