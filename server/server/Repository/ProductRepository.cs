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
                .Include(p => p.Seller)
                .ToList();

            return products.Select(p => new ProductCatalogDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Distance = p.Distance,
                SellerId = p.SellerId,
                SellerName = p.Seller.Name
            }).ToList();
        }
        public async Task AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> GetSellerByEmail(string email)
        {
            return await _context.Sellers.FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
