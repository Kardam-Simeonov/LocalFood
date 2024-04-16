namespace server.Dto
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
    }
}
