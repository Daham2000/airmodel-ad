using airmodel_ad.Models;

namespace airmodel_ad.Business.Interface
{
    public interface IOrderService
    {
        public bool AddOrder(OrderModel orderModel, List<OrderItem> orderItems, Guid cartId);
        public List<OrderModel> GetAllOrders(Guid userId);
        public List<OrderItem> GetAllOrderItems(Guid orderId);
    }
}
