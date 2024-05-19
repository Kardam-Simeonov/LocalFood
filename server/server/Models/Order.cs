using server.Dto;

namespace server.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; } 
        public double Longitude { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Floor { get; set; }
        public string? Apartment { get; set; }
        public string? Entrance { get; set; }
        public string? DeliveryNote { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
