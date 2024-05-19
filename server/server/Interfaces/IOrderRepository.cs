using server.Dto;
using server.Models;

namespace server.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(Order order);
        Task<ICollection<Order>> GetOrders();
        Task<ICollection<Order>> GetOrdersByVendorId(int id);
        Task<Order> GetOrderById(int id);
        Task<OrderProduct> GetOrderProductById(int orderId, int orderProductId);
        Task RemoveOrder(Order order);
        Task RemoveOrderProduct(int orderId, OrderProduct orderProduct);
        Task UpdateOrderProduct(OrderProduct product, OrderProductUpdateDto productDto);
    }
}
