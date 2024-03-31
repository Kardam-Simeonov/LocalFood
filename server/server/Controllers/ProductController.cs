using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Interfaces;
using server.Models;
using System.Web.Http.Controllers;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepostory)
        {
           _productRepository = productRepostory;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(ProductAddDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var seller = await _productRepository.GetSellerByEmail(productDto.SellerEmail);

            if (seller == null)
                return NotFound("Seller not found");

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                SellerId = seller.Id,
                Seller = seller
            };

            await _productRepository.AddProduct(product);

            return Ok(product);
        }

    }
}
