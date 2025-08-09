
using SystemPortal.Data.Context;

namespace SystemPortal.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        AppDbContext _appDbContext;
        public UnitOfWork(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        public async ValueTask<int> CommitAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
