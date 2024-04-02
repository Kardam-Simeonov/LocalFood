using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }
        public async Task AddOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
        public async Task<ICollection<Order>> GetOrdersByVendorId(int id)
        {
            return await _context.Orders
                .Include(o => o.Products)
                .Where(o => o.Products.Any(p => p.VendorId == id))
                .ToListAsync();
        }
    }
}
