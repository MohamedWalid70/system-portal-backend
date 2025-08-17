using SystemPortal.Data.Context;
using SystemPortal.Data.Entities;

namespace SystemPortal.Repository.Repositories.AppUserRepository
{
    public class AppUserRepository : IAppUserRepository
    {
        AppDbContext _dbContext;
        public AppUserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async ValueTask AddUserAsync(AppUser user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public void BeginEditingUser(AppUser user)
        {
            _dbContext.Users.Attach(user);
        }

        public void DeleteUser(AppUser user)
        {
            _dbContext.Users.Remove(user);
        }

        public IQueryable<AppUser> GetAll()
        {
            return _dbContext.Users;
        }

        public ValueTask<AppUser?> GetUserByIdAsync(int id)
        {
            return _dbContext.Users.FindAsync(id);
        }
    }
}
