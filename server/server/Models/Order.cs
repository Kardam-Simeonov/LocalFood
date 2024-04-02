using server.Dto;

namespace server.Models
{
    public class Order
    {
        public int Id { get; set; }
        public ICollection<Product> Products { get; set; }
        public string Address { get; set; }
    }
}
