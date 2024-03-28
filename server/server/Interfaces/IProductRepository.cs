using server.Models;

namespace server.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts(); 
    }
}
