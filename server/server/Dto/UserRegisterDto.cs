namespace server.Dto
{
    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
