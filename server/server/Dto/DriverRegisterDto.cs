namespace server.Dto
{
    public class DriverRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public IFormFile Image { get; set; }
    }
}
