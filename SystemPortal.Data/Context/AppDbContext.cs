using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SystemPortal.Data.Entities;

namespace SystemPortal.Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Otp>().HasOne<AppUser>().WithOne().HasForeignKey<AppUser>(appUser => appUser.OtpId);

            builder.Entity<Company>().HasMany<AppUser>().WithOne().HasForeignKey(appUser => appUser.CompanyId);
            
            base.OnModelCreating(builder);
        }
    }
}
