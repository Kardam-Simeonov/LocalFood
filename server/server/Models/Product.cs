namespace server.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Distance { get; set; }
        public Seller Seller { get; set; }
    }
}
