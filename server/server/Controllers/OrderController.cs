using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Interfaces;
using server.Models;
using server.Repository;

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

        [HttpPost]
        public async Task<IActionResult> Add(OrderAddDto orderDto)
        {
            var order = new Order
            {
                Address = orderDto.Address,
                Products = orderDto.Products.Select(productDto => new Product
                {
                    Id = productDto.Id,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Distance = productDto.Distance,
                    VendorId = productDto.VendorId
                }).ToList()
            };

            await _orderRepository.AddOrder(order);

            return Ok(order.Id);
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
