using SystemPortal.Data.Entities;

namespace SystemPortal.Repository.Repositories
{
    public interface ICompanyRepository
    {
        ValueTask AddCompanyAsync(Company company);
        ValueTask<Company?> GetCompanyByIdAsync(int id);
        IQueryable<Company> GetAll();
        void BeginEditingCompany(Company company);
        void DeleteCompany(Company company);

    }
}
