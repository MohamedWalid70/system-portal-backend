using Microsoft.EntityFrameworkCore;
using SystemPortal.Data.Entities;

namespace SystemPortal.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
}
