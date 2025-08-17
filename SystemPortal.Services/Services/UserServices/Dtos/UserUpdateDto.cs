using System.ComponentModel.DataAnnotations;

namespace SystemPortal.Services.Services.UserServices.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [RegularExpression(@"^[0-9]{10,15}$")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{6,}$")]
        public required string Password { get; set; }
    }
}
