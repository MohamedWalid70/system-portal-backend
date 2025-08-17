using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using SystemPortal.Data.Entities;
using SystemPortal.Services.Services.UserServices.Dtos;

namespace SystemPortal.Services.Services.UserServices
{
    public class UserServices : IUserServices
    {
        UserManager<AppUser> _userManager;
        public UserServices(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async ValueTask<Result> AddEmployeeAsync(UserSignUpDto userInfo)
        {
            AppUser appUser = new()
            {
                EnglishName = userInfo.Name,
                Email = userInfo.Email,
                PhoneNumber = userInfo.PhoneNumber,
                UserName = userInfo.Email,
                CompanyId = userInfo.CompanyId,
            };

            var identityResult = await _userManager.CreateAsync(appUser, userInfo.Password);

            if (!identityResult.Succeeded)
            {
                StringBuilder errorMessage = new();

                foreach (var error in identityResult.Errors)
                {
                    errorMessage.Append($"- Error code: {error.Code}, Description: {error.Description}\n");
                }

                return Result.Fail(errorMessage.ToString());
            }

            identityResult = await _userManager.AddToRoleAsync(appUser, "Employee");

            return CheckIdentityResult(identityResult);
        }

        public async ValueTask<Result<UserOutputDto>> GetUserAsync(int id)
        {
            var desiredUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (desiredUser != null)
            {
                UserOutputDto user = new()
                {
                    Email = desiredUser.Email!,
                    PhoneNumber = desiredUser.PhoneNumber,
                    EnglishName = desiredUser.EnglishName,
                    Id = desiredUser.Id
                };

                return Result.Ok(user);
            }

            return Result.Fail("User does not exist");
        }

        public IAsyncEnumerable<UserOutputDto> GetUsersAsync()
        {
            return _userManager.Users.Select(appUser => new UserOutputDto()
            {
                Email = appUser.Email!,
                EnglishName = appUser.EnglishName,
                Id = appUser.Id,
                PhoneNumber = appUser.PhoneNumber,

            }).AsAsyncEnumerable();
        }

        public async ValueTask<IEnumerable<UserOutputDto>> GetUsersExceptCompaniesAsync()
        {
            return (await _userManager.GetUsersInRoleAsync("Employee")).Select(user => new UserOutputDto()
            {
                Email = user.Email!,
                EnglishName = user.EnglishName,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
            });
        }

        public async ValueTask<Result> RemoveEmployeeAsync(int id)
        {
            var employeeToBeRemoved = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (employeeToBeRemoved == null)
            {
                return Result.Fail("Employee can not be removed as it does not exist");
            }

            var roles = await _userManager.GetRolesAsync(employeeToBeRemoved);

            foreach (var role in roles)
            {
                if (role.Equals("Employee"))
                {
                    var identityResult = await _userManager.DeleteAsync(employeeToBeRemoved);

                    return CheckIdentityResult(identityResult);
                }
            }

            return Result.Fail("User is not an employee");
        }

        public async ValueTask<Result<UserOutputDto>> UpdateUserAsync(UserUpdateDto user)
        {
            var userToBeUpdated = await _userManager.Users.FirstOrDefaultAsync(usr => usr.Id == user.Id);

            if (userToBeUpdated == null)
            {
                return Result.Fail("Update failed as the user does not exist");
            }

            userToBeUpdated.EnglishName = user.Name;
            userToBeUpdated.PhoneNumber = user.PhoneNumber;
 
            if(!user.Email.Equals(userToBeUpdated.Email))
            {
                userToBeUpdated.Email = user.Email;
                userToBeUpdated.NormalizedEmail = user.Email.ToUpper();
                userToBeUpdated.UserName = user.Email;
                userToBeUpdated.NormalizedEmail = user.Email.ToUpper();
            }

            var identityResult = await _userManager.UpdateAsync(userToBeUpdated);


            await _userManager.RemovePasswordAsync(userToBeUpdated);
            await _userManager.AddPasswordAsync(userToBeUpdated, user.Password);

            if (identityResult.Succeeded)
            {
                return Result.Ok(new UserOutputDto() { 
                    Email = userToBeUpdated.Email!, 
                    EnglishName= userToBeUpdated.EnglishName, 
                    Id = userToBeUpdated.Id, 
                    PhoneNumber = userToBeUpdated.PhoneNumber});
            }

            StringBuilder errorMessage = new();

            foreach (var error in identityResult.Errors)
            {
                errorMessage.Append($"- {error.Code} {error.Description}\n");
            }

            return Result.Fail(errorMessage.ToString());
        }


        private Result CheckIdentityResult(IdentityResult identityResult)
        {
            if (identityResult.Succeeded)
            {
                return Result.Ok();
            }

            StringBuilder errorMessage = new();

            foreach (var error in identityResult.Errors)
            {
                errorMessage.Append($"- {error.Code} {error.Description}\n");
            }

            return Result.Fail(errorMessage.ToString());
        }

        public IAsyncEnumerable<UserOutputDto> GetUsersByCompanyId(int companyId)
        {
            return _userManager.Users.Where(user => user.CompanyId == companyId).
                    Select(user => new UserOutputDto { 
                        Email = user.Email!,
                        EnglishName = user.EnglishName, 
                        Id = user.Id,
                        PhoneNumber = user.PhoneNumber,
                    }).AsAsyncEnumerable();
        }


    }
}
