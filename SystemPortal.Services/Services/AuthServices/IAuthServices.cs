using FluentResults;
using SystemPortal.Data.Dtos.CompanyDtos;
using SystemPortal.Data.Entities;

namespace SystemPortal.Services.Services.AuthServices
{
    public interface IAuthServices
    {
        ValueTask<Result<OutputCompanyDto>> CompanyLoginAsync(CompanyLoginDto companyLogin);
    }
}
