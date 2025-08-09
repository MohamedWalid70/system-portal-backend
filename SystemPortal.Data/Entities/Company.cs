using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace SystemPortal.Data.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public required string ArabicName { get; set; }
        public required string EnglishName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? LogoPath { get; set; }
        public string? LogoFileName { get; set; }
        public string? LogoContentType { get; set; }
        public required string Password { get; set; }
    }
}
