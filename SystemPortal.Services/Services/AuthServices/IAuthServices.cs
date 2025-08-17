using FluentResults;
using SystemPortal.Services.Services.AuthServices.Dtos;
using SystemPortal.Services.Services.CompanyServices.Dtos;
using SystemPortal.Services.Services.UserServices.Dtos;

namespace SystemPortal.Services.Services.AuthServices
{
    public interface IAuthServices
    {
        ValueTask<Result<UserOutputDto>> AdminLoginAsync(LoginDto userLogin);
        ValueTask<Result<CompanyOutputDto>> CompanyLoginAsync(LoginDto companyLogin);
    }
}
