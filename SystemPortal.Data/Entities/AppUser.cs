using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPortal.Data.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public required string EnglishName { get; set; }
        public Guid? OtpId { get; set; }
        public int? OtpValue { get; set; }
        public int? CompanyId { get; set; }
    }
}
