using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Interfaces;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetOrdersByVendor(int vendorId)
        {
            var orders = await _orderRepository.GetOrdersByVendorId(vendorId);
            var filteredOrders = orders.Select(o => new Order
            {
                Id = o.Id,
                Products = o.Products.Where(p => p.VendorId == vendorId).ToList(),
                Address = o.Address
            }).ToList();

            return Ok(filteredOrders);
        }
    }
}
