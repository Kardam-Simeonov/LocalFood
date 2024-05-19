namespace server.Dto
{
    public class VendorRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IFormFile Image { get; set; }
    }
}
