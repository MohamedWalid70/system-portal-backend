using FluentResults;
using Microsoft.AspNetCore.Http;
using SystemPortal.Data.Entities;
using SystemPortal.Services.Services.CompanyServices.Dtos;

namespace SystemPortal.Services.Services.CompanyServices
{
    public interface ICompanyServices
    {
        IAsyncEnumerable<CompanyOutputDto> GetCompaniesAsync();
        ValueTask<Result> RegisterCompanyAsync(CompanySignUpDto company);
        ValueTask<Result> UnregisterCompanyAsync(int id);
        ValueTask<Result<CompanyOutputDto>> GetCompanyInfo(int id);
        ValueTask<Result<(string, string, string)>> GetCompanyLogoAsync(int id);
        ValueTask<Result<string>> UploadCompanyLogo(IFormFile file);
        ValueTask<Result<CompanyOutputDto>> UpdateCompanyInfo(CompanyUpdateDto company);

    }
}
