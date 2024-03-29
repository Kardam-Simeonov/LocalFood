using server.Dto;

namespace server.Interfaces
{
    public interface IProductRepository
    {
        ICollection<ProductCatalogDto> GetProducts(); 
    }
}
