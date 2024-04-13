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
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductAddDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendor = await _productRepository.GetProductVendorById(productDto.VendorId);

            if (vendor == null)
                return NotFound("Vendor not found");

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                VendorId = vendor.Id,
                Vendor = vendor
            };

            if (productDto.Image != null && productDto.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await productDto.Image.CopyToAsync(memoryStream);
                    product.Image = memoryStream.ToArray();
                }
            }

            await _productRepository.AddProduct(product);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
                return NotFound("Product not found");

            await _productRepository.RemoveProduct(product);

            return Ok();
        }

    }
}
