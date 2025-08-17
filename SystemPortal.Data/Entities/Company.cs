using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using SystemPortal.Data.Entities;

namespace SystemPortal.Data.Entities
{
    public class Company : AppUser
    {
        public required string ArabicName { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? LogoPath { get; set; }
        public string? LogoFileName { get; set; }
        public string? LogoContentType { get; set; }
    }
}
