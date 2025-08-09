using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SystemPortal.Data.Dtos.CompanyDtos
{
    public class CompanyDto
    {
        [Required]
        public required string ArabicName { get; set; }
        
        [Required]
        public required string EnglishName { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [RegularExpression(@"^$|^\+?\d{1,3}?[-.\s]?\(?\d{1,4}\)?([-.\s]?\d{1,4}){1,3}$")]
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$")]
        public required string Password { get; set; }
    }
}
