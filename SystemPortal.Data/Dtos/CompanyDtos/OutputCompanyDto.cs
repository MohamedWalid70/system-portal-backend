using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SystemPortal.Data.Dtos.CompanyDtos
{
    public class OutputCompanyDto
    {
        public int Id { get; set; }
        public required string ArabicName { get; set; }
        public required string EnglishName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
