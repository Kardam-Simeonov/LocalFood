using server.Models;

namespace server.Interfaces
{
    public interface IOrderRepository
    {
        Task<ICollection<Order>> GetOrdersByVendorId(int id);
    }
}
