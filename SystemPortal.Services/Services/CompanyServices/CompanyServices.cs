using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SystemPortal.Data.Entities;
using SystemPortal.Data.Security.Cryptography;
using SystemPortal.Repository.Repositories.CompanyRepository;
using SystemPortal.Repository.UnitOfWork;
using SystemPortal.Services.Services.CompanyServices.Dtos;
using SystemPortal.Services.Services.OtpServices;

namespace SystemPortal.Services.Services.CompanyServices
{
    public class CompanyServices : ICompanyServices
    {
        ICompanyRepository _companyRepository;
        UserManager<AppUser> _userManager;
        IUnitOfWork _unitOfWork;
        IOtpServices _otpServices;

        public CompanyServices(ICompanyRepository companyRepository, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IOtpServices otpServices)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _otpServices = otpServices;
        }

        public IAsyncEnumerable<CompanyOutputDto> GetCompaniesAsync()
        {
            return _companyRepository.GetAll().
                Select(company => new CompanyOutputDto()
                {
                    ArabicName = company.ArabicName,
                    Email = company.Email!,
                    EnglishName = company.EnglishName,
                    Id = company.Id,
                    PhoneNumber = company.PhoneNumber,
                    WebsiteUrl = company.WebsiteUrl,
                    IsEmailVerified = company.EmailConfirmed,
                    OtpValue = company.OtpValue,
                }).
                AsAsyncEnumerable();
        }

        public async ValueTask<Result<(string, string, string)>> GetCompanyLogoAsync(int id)
        {
            Company? company = await _companyRepository.GetCompanyByIdAsync(id);

            if (company == null)
            {
                return Result.Fail("There is no company with such an Id");
            }

            if(company.LogoPath == null || company.LogoFileName == null || company.LogoContentType == null)
            {
                return Result.Fail("No associated logo");
            }

            return Result.Ok((company.LogoPath, company.LogoFileName, company.LogoContentType));
        }

        public async ValueTask<Result<CompanyOutputDto>> GetCompanyInfo(int id)
        {
            Company? desiredCompany = await _companyRepository.GetCompanyByIdAsync(id);

            if(desiredCompany == null)
            {
                return Result.Fail("Company is not found");
            }

            CompanyOutputDto company = new()
            {
                ArabicName = desiredCompany.ArabicName,
                Email = desiredCompany.Email!,
                EnglishName = desiredCompany.EnglishName,
                Id = desiredCompany.Id,
                PhoneNumber = desiredCompany.PhoneNumber,
                WebsiteUrl = desiredCompany.WebsiteUrl,
                IsEmailVerified = desiredCompany.EmailConfirmed,
                OtpValue = desiredCompany.OtpValue,
            };

            return Result.Ok(company);
            
        }

        public async ValueTask<Result> RegisterCompanyAsync(CompanySignUpDto company)
        {

            var otp = await _otpServices.GetOtpByIdAsync(company.OtpId);

            if (otp != null && otp.IsVerified)
            {

                var duplicateCompany = await _companyRepository.GetAll().FirstOrDefaultAsync(comp => comp.Email == company.Email);

                if (duplicateCompany == null)
                {

                    if (company != null)
                    {

                        Company companyInfo = new()
                        {
                            ArabicName = company.ArabicName,
                            Email = company.Email,
                            EnglishName = company.EnglishName,
                            LogoPath = company.LogoPath,
                            PhoneNumber = company.PhoneNumber,
                            PasswordHash = await Cryptography.GetPasswordHash(company.Password),
                            WebsiteUrl = company.WebsiteUrl,
                            LogoFileName = company.LogoFileName,
                            LogoContentType = company.LogoContentType,
                            NormalizedEmail = company.Email.ToUpper(),
                            UserName = company.Email,
                            NormalizedUserName = company.Email.ToUpper(),
                            SecurityStamp = Guid.NewGuid().ToString(),
                            OtpId = otp.Id,
                            EmailConfirmed = true
                        };

                        await _companyRepository.AddCompanyAsync(companyInfo);
                        await _unitOfWork.CommitAsync();

                        var identityResult = await _userManager.AddToRoleAsync(companyInfo, "Company");


                        if (!identityResult.Succeeded)
                        {
                            return Result.Fail("Role addition failure!");
                        }


                        return Result.Ok();
                    }
                }
                else
                {
                    return Result.Fail("There is already a company with the same email");
                }
            }
            else
            {
                return Result.Fail("OTP was not verified");
            }

            return Result.Fail("Unrecognized Error");
        }

        public async ValueTask<Result<CompanyOutputDto>> UpdateCompanyInfo(CompanyUpdateDto company)
        {
            Company? companyToBeUpdated = await _companyRepository.GetCompanyByIdAsync(company.Id);

            if (companyToBeUpdated != null)
            {
                _companyRepository.BeginEditingCompany(companyToBeUpdated);

                companyToBeUpdated.PasswordHash = await Cryptography.GetPasswordHash(company.Password);
                companyToBeUpdated.ArabicName = company.ArabicName;
                companyToBeUpdated.EnglishName = company.EnglishName;
                companyToBeUpdated.PhoneNumber = company.PhoneNumber;
                companyToBeUpdated.WebsiteUrl = company.WebsiteUrl;

                if (!company.Email.Equals(companyToBeUpdated.Email))
                {
                    companyToBeUpdated.Email = company.Email;
                    companyToBeUpdated.NormalizedEmail = company.Email.ToUpper();
                    companyToBeUpdated.UserName = company.Email;
                    companyToBeUpdated.NormalizedEmail = companyToBeUpdated.UserName?.ToUpper();

                }

                await _unitOfWork.CommitAsync();
                return Result.Ok(new CompanyOutputDto { 
                    ArabicName = companyToBeUpdated.ArabicName,
                    Email = companyToBeUpdated.Email,
                    EnglishName = companyToBeUpdated.EnglishName,
                    PhoneNumber = companyToBeUpdated.PhoneNumber,
                    WebsiteUrl = companyToBeUpdated.WebsiteUrl,
                    Id = companyToBeUpdated.Id,
                    IsEmailVerified = companyToBeUpdated.EmailConfirmed,
                    OtpValue = companyToBeUpdated.OtpValue
                });
            }
            return Result.Fail("The Company does not exist");
        }

        public async ValueTask<Result<string>> UploadCompanyLogo(IFormFile file)
        {
            string? dbPath;

            if (file.FileName != null)
            {

                if (file.Length == 0 || !file.ContentType.StartsWith("image"))
                {
                    return Result.Fail("Unsupported file type!");
                }

                var folderName = Path.Combine("Resources", "Logos");

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var fullPath = Path.Combine(pathToSave, file.FileName);

                dbPath = Path.Combine(folderName, file.FileName);

                //if (System.IO.File.Exists(fullPath))
                //{
                //    return Result.Fail("The file already exists!");
                //}

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Result.Ok(dbPath);
            }

            return Result.Fail("Error with the uploaded file");
        }

        public async ValueTask<Result> UnregisterCompanyAsync(int id)
        {
            Company? company = await _companyRepository.GetCompanyByIdAsync(id);

            if(company != null)
            {
                _companyRepository.DeleteCompany(company);
                await _unitOfWork.CommitAsync();
                return Result.Ok();
            }
            return Result.Fail("The company does not exist");
        }

     
    }
}
