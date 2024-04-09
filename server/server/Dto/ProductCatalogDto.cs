namespace server.Dto
{
    public class ProductCatalogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Distance { get; set; }
        public byte[] Image { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
    }
}
