using FluentResults;
using SystemPortal.Data.Entities;
using SystemPortal.Services.Services.UserServices.Dtos;

namespace SystemPortal.Services.Services.UserServices
{
    public interface IUserServices
    {
        IAsyncEnumerable<UserOutputDto> GetUsersAsync();
        ValueTask<IEnumerable<UserOutputDto>> GetUsersExceptCompaniesAsync();
        ValueTask<Result> AddEmployeeAsync(UserSignUpDto userInfo);
        ValueTask<Result> RemoveEmployeeAsync(int id);
        ValueTask<Result<UserOutputDto>> UpdateUserAsync(UserUpdateDto user);
        ValueTask<Result<UserOutputDto>> GetUserAsync(int id);
        IAsyncEnumerable<UserOutputDto> GetUsersByCompanyId(int companyId);
    }
}
