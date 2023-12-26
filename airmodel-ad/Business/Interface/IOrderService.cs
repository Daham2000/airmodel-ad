using airmodel_ad.Models;

namespace airmodel_ad.Business.Interface
{
    public interface IOrderService
    {
        public bool AddOrder(OrderModel orderModel, List<OrderItem> orderItems, Guid cartId);
        public List<OrderModel> GetAllOrders(Guid userId);
        public OrderModel GetOrderByID(Guid oId);
        public List<OrderModel> GetAllPendingOrders();
        public List<OrderModel> GetAllUserOrders();
        public List<OrderItem> GetAllOrderItems(Guid orderId);
        public List<OrderModel> GetOrderByStatus(string status);
        public List<OrderModel> GetOrderByDateTime(DateTime date);
        public bool ChangeOrderState(Guid orderId, string status);
    }
}
