using SystemPortal.Data.Entities;

namespace SystemPortal.Repository.Repositories.AppUserRepository
{
    public interface IAppUserRepository
    {
        ValueTask AddUserAsync(AppUser user);
        ValueTask<AppUser?> GetUserByIdAsync(int id);
        IQueryable<AppUser> GetAll();
        void BeginEditingUser(AppUser user);
        void DeleteUser(AppUser user);
    }
}
