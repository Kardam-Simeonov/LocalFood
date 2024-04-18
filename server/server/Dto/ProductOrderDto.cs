namespace server.Dto
{
    public class ProductOrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
    }
}
