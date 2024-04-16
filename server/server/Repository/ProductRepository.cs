using Azure.Core;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<ProductCatalogDto> GetProducts()
        {
            var products = _context.Products
                .Include(p => p.Vendor)
                .ToList();

            return products.Select(p => new ProductCatalogDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Latitude = p.Vendor.Latitude,
                Longitude = p.Vendor.Longitude,
                Image = p.Image,
                VendorId = p.VendorId,
                VendorName = p.Vendor.Name,
                VendorImage = p.Vendor.Image,
            }).ToList();
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductById(Product product, ProductAddDto productDto)
        {
            product.Name = productDto.Name;
            product.Price = productDto.Price;

            if (productDto.Image != null && productDto.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await productDto.Image.CopyToAsync(memoryStream);
                    product.Image = memoryStream.ToArray();
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
