namespace SystemPortal.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ValueTask<int> CommitAsync();
    }
}
