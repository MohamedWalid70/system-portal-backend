using FluentResults;
using Microsoft.EntityFrameworkCore;
using SystemPortal.Data.Dtos.CompanyDtos;
using SystemPortal.Data.Entities;
using SystemPortal.Data.Security.Cryptography;
using SystemPortal.Repository.Repositories;

namespace SystemPortal.Services.Services.AuthServices
{
    public class AuthServices : IAuthServices
    {
        ICompanyRepository _companyRepository;

        public AuthServices(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async ValueTask<Result<OutputCompanyDto>> CompanyLoginAsync(CompanyLoginDto companyLogin)
        {
            string email = companyLogin.Email.ToLower();

            Company? company = await _companyRepository.GetAll().FirstOrDefaultAsync(comp => comp.Email == email);

            string passHash = await Cryptography.GetPasswordHash(companyLogin.Password);

            if (company == null)
            {
                // This line is for avoiding timing attacks
                company = _companyRepository.GetAll().FirstOrDefault(comp => comp.Password == passHash);

                return Result.Fail("Email address does not exist");
            }
            else
            {
                company = _companyRepository.GetAll().FirstOrDefault(comp => comp.Email == email && comp.Password == passHash);
                
                if(company == null)
                {
                    return Result.Fail("Email address is found but password is not");
                }

                OutputCompanyDto companyDto = new OutputCompanyDto()
                {
                    ArabicName = company.ArabicName,
                    Email = company.Email,
                    EnglishName = company.EnglishName,
                    PhoneNumber = company.PhoneNumber,
                    WebsiteUrl = company.WebsiteUrl,
                    Id = company.Id
                };

                return Result.Ok(companyDto);
            }
        }
    }
}
