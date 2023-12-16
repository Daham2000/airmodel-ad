﻿using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace airmodel_ad.Business.Services
{
    public class OrderService : IOrderService
    {
        AppDbContext appDbContext;
        public OrderService(AppDbContext context)
        {
            appDbContext = context;
        }
        public bool AddOrder(OrderModel orderModel, List<OrderItem> orderItems, Guid cartId)
        {
            try {
                List<CartItemModel> cartItemModels = appDbContext.cartItems.Where((u) => u.cartId == cartId).ToList();
                Debug.WriteLine(cartItemModels.Count());
                foreach (CartItemModel cartItemModel in cartItemModels)
                {
                    appDbContext.cartItems.Remove(cartItemModel);
                }
                appDbContext.orders.Add(orderModel);
                appDbContext.SaveChanges();
                for (int i = 0; i<=orderItems.Count()-1; i++)
                {
                    orderItems[i].oId = orderModel.oId;
                    appDbContext.orderItem.Add(orderItems[i]);
                    ProductModel? productModel = appDbContext.products.Where((p) => p.productId == orderItems[i].productId).FirstOrDefault();
                    productModel.productQty = productModel.productQty - orderItems[i].qty;
                    appDbContext.SaveChanges();
                    if (orderItems[i].varientOptionId != new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        VarientOptionModel? varientOptionModel = appDbContext.varientOption.Where((p) => p.varientOptionId == orderItems[i].varientOptionId).FirstOrDefault();
                        varientOptionModel.varientQty = varientOptionModel.varientQty- orderItems[i].qty;
                        appDbContext.SaveChanges();
                    }
                }
                appDbContext.SaveChanges();
                return true;
            }
            catch(Exception e) {
                Debug.WriteLine($"Error: {e}"); 
                return false;
            }
        }

        public List<OrderItem> GetAllOrderItems(Guid orderId)
        {
            List <OrderItem> orderItems = new List<OrderItem>();  
            try {
                orderItems = appDbContext.orderItem.Where((o) => o.oId== orderId).ToList();
                for(int i=0; i<=orderItems.Count(); i++)
                {
                    orderItems[i].products = appDbContext.products.Where((p) => p.productId == orderItems[i].productId).FirstOrDefault();  
                } 
                return orderItems;
            } catch (Exception e) {
                return orderItems;
            }
        }

        public List<OrderModel> GetAllOrders(Guid userId)
        {
            List<OrderModel> orderModels = new List<OrderModel>();
            try
            {
                orderModels = appDbContext.orders.Where((o) => o.userId == userId).ToList();
                return orderModels;
            }
            catch (Exception e)
            {
                return orderModels;
            }
        }
    }
}
