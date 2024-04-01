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
                Distance = p.Distance,
                SellerId = p.VendorId,
                SellerName = p.Vendor.Name
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
        public async Task RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<Vendor> GetSellerById(int sellerId)
        {
            return await _context.Vendors.FirstOrDefaultAsync(s => s.Id == sellerId);
        }
    }
}
