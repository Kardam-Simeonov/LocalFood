using server.Dto;
using server.Models;

namespace server.Interfaces
{
    public interface IProductRepository
    {
        ICollection<ProductCatalogDto> GetProducts();
        Task AddProduct(Product product);
        Task<Seller> GetSellerById(int sellerId);
    }
}
