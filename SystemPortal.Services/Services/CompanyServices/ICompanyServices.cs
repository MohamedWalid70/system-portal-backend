using FluentResults;
using SystemPortal.Data.Dtos.CompanyDtos;
using SystemPortal.Data.Entities;

namespace SystemPortal.Services.services.CompanyServices
{
    public interface ICompanyServices
    {
        ValueTask<List<Company>> GetCompaniesAsync();
        ValueTask<Result> RegisterCompanyAsync(CompanyDto company);
        ValueTask<bool> UnregisterCompanyAsync(int id);
        ValueTask<Result<Company>> GetCompanyInfo(int id);
        ValueTask<Result<(string?, string?, string?)>> GetCompanyLogoAsync(int id);
    }
}
