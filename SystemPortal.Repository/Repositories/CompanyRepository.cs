using SystemPortal.Data.Context;
using SystemPortal.Data.Entities;

namespace SystemPortal.Repository.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private AppDbContext _appDbContext;
        public CompanyRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }
        public async ValueTask AddCompanyAsync(Company company)
        {
            await _appDbContext.Companies.AddAsync(company);
        }

        public void DeleteCompany(Company company)
        {
            _appDbContext.Companies.Remove(company);
        }

        public void BeginEditingCompany(Company company)
        {
            _appDbContext.Attach(company);
        }

        public IQueryable<Company> GetAll()
        {
            return _appDbContext.Companies;
        }

        public async ValueTask<Company?> GetCompanyByIdAsync(int id)
        {
            return await _appDbContext.Companies.FindAsync(id);
        }
    }
}
