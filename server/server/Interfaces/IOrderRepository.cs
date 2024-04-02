using server.Models;

namespace server.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(Order order);
        Task<ICollection<Order>> GetOrdersByVendorId(int id);
    }
}
