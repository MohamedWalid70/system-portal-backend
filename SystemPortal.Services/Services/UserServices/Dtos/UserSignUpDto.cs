using System.ComponentModel.DataAnnotations;
using SystemPortal.Data.Entities;

namespace SystemPortal.Services.Services.UserServices.Dtos
{
    public class UserSignUpDto
    {
        [Required]
        public required string Name { get; set; }
        
        [Required, EmailAddress]
        public required string Email { get; set; }

        [RegularExpression(@"^[0-9]{10,15}$")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{6,}$")]
        public required string Password { get; set; }
        public int? CompanyId { get; set; }
    }
}
