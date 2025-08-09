using FluentResults;
using Microsoft.EntityFrameworkCore;
using SystemPortal.Data.Dtos.CompanyDtos;
using SystemPortal.Data.Entities;
using SystemPortal.Data.Security.Cryptography;
using SystemPortal.Repository.Repositories;
using SystemPortal.Repository.UnitOfWork;

namespace SystemPortal.Services.services.CompanyServices
{
    public class CompanyServices : ICompanyServices
    {
        ICompanyRepository _companyRepository;
        IUnitOfWork _unitOfWork;

        public CompanyServices(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<List<Company>> GetCompaniesAsync()
        {
            return await _companyRepository.GetAll().ToListAsync();
        }

        public async ValueTask<Result<(string?, string?, string?)>> GetCompanyLogoAsync(int id)
        {
            Company? company = await _companyRepository.GetCompanyByIdAsync(id);

            if (company == null)
            {
                return Result.Fail("There is no company with such an Id");
            }

            return Result.Ok((company.LogoPath, company.LogoContentType, company.LogoFileName));
        }

        public async ValueTask<Result<Company>> GetCompanyInfo(int id)
        {
            Company? desiredCompany = await _companyRepository.GetCompanyByIdAsync(id);

            if(desiredCompany == null)
            {
                return Result.Fail("Company is not found");
            }
            return Result.Ok(desiredCompany);
            
        }

        public async ValueTask<Result> RegisterCompanyAsync(CompanyDto company)
        {
            var duplicateCompany = await _companyRepository.GetAll().FirstOrDefaultAsync(comp => comp.Email == company.Email);

            if (duplicateCompany == null)
            {
                string? dbPath;

                if (company?.LogoFile?.FileName != null)
                {

                    if (company.LogoFile?.Length == 0 || !company.LogoFile.ContentType.StartsWith("image"))
                    {
                        return Result.Fail("Unsupported file type!");
                    }

                    var folderName = Path.Combine("Resources", "Logos");

                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var fullPath = Path.Combine(pathToSave, company.LogoFile.FileName);

                    dbPath = Path.Combine(folderName, company.LogoFile.FileName);

                    //if (System.IO.File.Exists(fullPath))
                    //{
                    //    return Result.Fail("The file already exists!");
                    //}

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await company.LogoFile.CopyToAsync(stream);
                    }
                }
                else
                {
                    dbPath = null;
                }

                if (company != null)
                {

                    Company companyInfo = new()
                    {
                        ArabicName = company.ArabicName,
                        Email = company.Email.ToLower(),
                        EnglishName = company.EnglishName,
                        LogoPath = dbPath,
                        PhoneNumber = company?.PhoneNumber,
                        Password = await Cryptography.GetPasswordHash(company.Password),
                        WebsiteUrl = company?.WebsiteUrl,
                        LogoFileName = company?.LogoFile?.Name,
                        LogoContentType = company?.LogoFile?.ContentType
                    };

                    await _companyRepository.AddCompanyAsync(companyInfo);
                    await _unitOfWork.CommitAsync();
                    return Result.Ok();
                }
            }
            else
            {
                return Result.Fail("There is already a company with the same email");
            }
            return Result.Fail("Unrecognized Error");
        }

        public async ValueTask<bool> UnregisterCompanyAsync(int id)
        {
            Company? company = await _companyRepository.GetCompanyByIdAsync(id);

            if(company != null)
            {
                _companyRepository.DeleteCompany(company);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }

     
    }
}
