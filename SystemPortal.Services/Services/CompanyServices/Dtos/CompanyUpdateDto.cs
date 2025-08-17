using System.ComponentModel.DataAnnotations;

namespace SystemPortal.Services.Services.CompanyServices.Dtos
{
    public class CompanyUpdateDto
    {
        public int Id { get; set; }
        [Required]
        public required string ArabicName { get; set; }

        [Required]
        public required string EnglishName { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [RegularExpression(@"^$|^\+?\d{1,3}?[-.\s]?\(?\d{1,4}\)?([-.\s]?\d{1,4}){1,3}$")]
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }

        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{6,}$")]
        public required string Password { get; set; }
    }
}
