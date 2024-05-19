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
        public async Task<ICollection<Order>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync();
        }
        public async Task<ICollection<Order>> GetOrdersByVendorId(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Where(o => o.OrderProducts.Any(op => op.Product.VendorId == id))
                .ToListAsync();
        }
        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<OrderProduct> GetOrderProductById(int orderId, int orderProductId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return order.OrderProducts
                .FirstOrDefault(o => o.Id == orderProductId);
        }
        public async Task RemoveOrder(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOrderProduct(int orderId, OrderProduct orderProduct)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            order.OrderProducts.Remove(orderProduct);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderProduct(OrderProduct product, OrderProductUpdateDto productDto)
        {
            product.ReadyForPickup = productDto.ReadyForPickup;

            await _context.SaveChangesAsync();
        }
    }
}
