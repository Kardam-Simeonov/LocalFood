namespace server.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Distance { get; set; }
        public byte[] Image { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
