namespace SystemPortal.Services.Services.CompanyServices.Dtos
{
    public class CompanyOutputDto
    {
        public int Id { get; set; }
        public required string ArabicName { get; set; }
        public required string EnglishName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public int? OtpValue { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
