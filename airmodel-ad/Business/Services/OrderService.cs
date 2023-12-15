using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using System.Collections.Generic;

namespace airmodel_ad.Business.Services
{
    public class OrderService : IOrderService
    {
        AppDbContext appDbContext;
        public OrderService(AppDbContext context)
        {
            appDbContext = context;
        }
        public bool AddOrder(OrderModel orderModel, List<OrderItem> orderItems)
        {
            try {
                appDbContext.orders.Add(orderModel);
                appDbContext.SaveChanges();
                return true;
            }
            catch(Exception e) {
                return false;
            }
        }

        public List<OrderItem> GetAllOrderItems(Guid orderId)
        {
            List <OrderItem> orderItems = new List<OrderItem>();  
            try {
                return appDbContext.orderItem.Where((o) => o.oId== orderId).ToList();
            } catch (Exception e) {
                return orderItems;
            }
        }

        public List<OrderModel> GetAllOrders(Guid userId)
        {
            List<OrderModel> orderModels = new List<OrderModel>();
            try
            {
                return appDbContext.orders.Where((o) => o.userId == userId).ToList();
            }
            catch (Exception e)
            {
                return orderModels;
            }
        }
    }
}
