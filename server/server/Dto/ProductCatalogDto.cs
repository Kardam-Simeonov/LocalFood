namespace server.Dto
{
    public class ProductCatalogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Distance { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
    }
}
