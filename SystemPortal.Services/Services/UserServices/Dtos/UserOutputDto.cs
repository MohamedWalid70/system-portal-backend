namespace SystemPortal.Services.Services.UserServices.Dtos
{
    public class UserOutputDto
    {
        public int Id { get; set; }
        public required string EnglishName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
