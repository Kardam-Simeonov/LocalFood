using server.Dto;

namespace server.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
