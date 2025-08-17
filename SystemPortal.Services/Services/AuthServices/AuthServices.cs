using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SystemPortal.Data.Entities;
using SystemPortal.Data.Security.Cryptography;
using SystemPortal.Repository.Repositories.CompanyRepository;
using SystemPortal.Services.Services.AuthServices.Dtos;
using SystemPortal.Services.Services.CompanyServices.Dtos;
using SystemPortal.Services.Services.UserServices.Dtos;

namespace SystemPortal.Services.Services.AuthServices
{
    public class AuthServices : IAuthServices
    {
        ICompanyRepository _companyRepository;
        UserManager<AppUser> _userManager;

        public AuthServices(ICompanyRepository companyRepository, UserManager<AppUser> userManager)
        {
            _companyRepository = companyRepository;
            _userManager = userManager;
        }

        public async ValueTask<Result<CompanyOutputDto>> CompanyLoginAsync(LoginDto companyLogin)
        {
            string email = companyLogin.Email.ToUpper();

            Company? company = await _companyRepository.GetAll().FirstOrDefaultAsync(comp => comp.NormalizedEmail == email);

            string passHash = await Cryptography.GetPasswordHash(companyLogin.Password);

            if (company == null)
            {
                // This line is for avoiding timing attacks
                company = _companyRepository.GetAll().FirstOrDefault(comp => comp.PasswordHash == passHash);

                return Result.Fail("Email address does not exist");
            }
            else
            {
                company = _companyRepository.GetAll().FirstOrDefault(comp => comp.NormalizedEmail == email && comp.PasswordHash == passHash);
                
                if(company == null)
                {
                    return Result.Fail("Email address is found but password is not");
                }

                CompanyOutputDto companyDto = new CompanyOutputDto()
                {
                    ArabicName = company.ArabicName,
                    Email = company.Email!,
                    EnglishName = company.EnglishName,
                    PhoneNumber = company.PhoneNumber,
                    WebsiteUrl = company.WebsiteUrl,
                    Id = company.Id
                };

                return Result.Ok(companyDto);
            }
        }

        public async ValueTask<Result<UserOutputDto>> AdminLoginAsync(LoginDto userLogin)
        {
            AppUser? user = await _userManager.FindByEmailAsync(userLogin.Email);

            if (user == null)
            {
                return Result.Fail("Email address does not exist");
            }
            else
            {
                var result = await _userManager.CheckPasswordAsync(user, userLogin.Password);

                if (!result)
                {
                    return Result.Fail("Incorrect password");
                }

                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return Result.Fail("User is not an admin!");
                }

                UserOutputDto userOutput = new()
                {
                    Email = user.Email!,
                    EnglishName= user.EnglishName,
                    PhoneNumber = user.PhoneNumber,
                    Id= user.Id
                };

                return Result.Ok(userOutput);
            }
        }
    }
}
