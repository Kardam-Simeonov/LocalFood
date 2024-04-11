using server.Models;

namespace server.Dto
{
    public class OrderAddDto
    {
        public List<ProductDto> Products { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
