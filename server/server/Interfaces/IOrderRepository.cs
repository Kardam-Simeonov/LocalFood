using server.Models;

namespace server.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(Order order);
        Task<ICollection<Order>> GetOrdersByVendorId(int id);
        Task<Order> GetOrderById(int id);
        Task RemoveOrder(Order order);
    }
}
