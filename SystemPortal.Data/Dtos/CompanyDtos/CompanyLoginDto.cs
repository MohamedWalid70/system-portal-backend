using System.ComponentModel.DataAnnotations;

namespace SystemPortal.Data.Dtos.CompanyDtos
{
    public class CompanyLoginDto
    {
        [Required,EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
