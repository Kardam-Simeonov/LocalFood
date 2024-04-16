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
        private readonly IVendorRepository _vendorRepository;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
                return NotFound("Product not found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductAddDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendor = await _vendorRepository.GetVendorById(productDto.VendorId);

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

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductAddDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepository.GetProductById(id);

            if (product == null)
                return NotFound("Product not found");

            await _productRepository.UpdateProductById(product, productDto);

            return Ok();
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
