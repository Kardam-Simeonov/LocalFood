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

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetOrders();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderAddDto orderDto)
        {
            var order = new Order
            {
                Address = orderDto.Address,
                Latitude = orderDto.Latitude,
                Longitude = orderDto.Longitude,
                OrderProducts = orderDto.Products.Select(productDto => new OrderProduct
                {
                    ProductId = productDto.Id
                }).ToList()
            };

            await _orderRepository.AddOrder(order);

            return Ok(order.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var order = await _orderRepository.GetOrderById(id);

            if (order == null)
                return NotFound("Product not found");

            await _orderRepository.RemoveOrder(order);

            return Ok();
        }

        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetOrdersByVendor(int vendorId)
        {
            var orders = await _orderRepository.GetOrdersByVendorId(vendorId);
            var filteredOrders = orders.Select(o => new Order
            {
                Id = o.Id,
                OrderProducts = o.OrderProducts.Where(op => op.Product.VendorId == vendorId).ToList(),
                Address = o.Address,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
            }).ToList();

            return Ok(filteredOrders);
        }
    }
}
