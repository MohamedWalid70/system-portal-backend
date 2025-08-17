using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using SystemPortal.Services.Services.CompanyServices;
using SystemPortal.Services.Services.CompanyServices.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemPortal.Api.Controllers
{
    [Route("v1/api/system-portal/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        ICompanyServices _companyServices;
        public CompanyController(ICompanyServices companyServices)
        {
            _companyServices = companyServices;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IAsyncEnumerable<CompanyOutputDto> GetCompanies()
        {
            return _companyServices.GetCompaniesAsync();

        }

        [HttpGet("{id}")]
        public async ValueTask<ActionResult> GetCompany(int id)
        {
            Result<CompanyOutputDto> result = await _companyServices.GetCompanyInfo(id);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound();
        }

        [HttpPost("new")]
        public async ValueTask<ActionResult> AddCompany(CompanySignUpDto company)
        {

            var result = await _companyServices.RegisterCompanyAsync(company);

            if (result.IsSuccess)
            {
                return Created();
            }

            StringBuilder errorMessage = new();

            foreach (var error in result.Errors)
            {
                errorMessage.Append($"- {error.Message}\n");
            }

            return BadRequest(errorMessage.ToString());
        }

        //[Authorize(Roles = "Admin")]
        [HttpPatch]
        public async ValueTask<ActionResult> UpdateCompany(CompanyUpdateDto company)
        {
            var result = await _companyServices.UpdateCompanyInfo(company);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            StringBuilder errorMessage = new();

            foreach (var error in result.Errors)
            {
                errorMessage.Append($"- {error.Message}\n");
            }

            return BadRequest(errorMessage.ToString());
        }

        [HttpGet("logo/{id}")]
        public async ValueTask<ActionResult> DownloadLogo(int id)
        {
            var result = await _companyServices.GetCompanyLogoAsync(id);

            if (result.IsSuccess)
            {
                if (!System.IO.File.Exists(result.Value.Item1))
                    return NotFound();

                var fileBytes = System.IO.File.ReadAllBytes(result.Value.Item1);

                return File(fileBytes, result.Value.Item3, result.Value.Item2);

            }
            else if (result.IsFailed && result.HasError(err => err.Message.Contains("associated")))
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost("logo")]
        public async ValueTask<ActionResult> UploadLogo(IFormFile logoFile)
        {
            var uploadResult = await _companyServices.UploadCompanyLogo(logoFile);

            if (uploadResult.IsSuccess)
            {
                return Ok(new PathContainerDto() { logoPath = uploadResult.Value });
            }

            if (uploadResult.HasError(err => err.Message.Contains("Unsupported"))) {

                return new StatusCodeResult(StatusCodes.Status415UnsupportedMediaType);
            }

            return BadRequest();
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async ValueTask<ActionResult> RemoveCompany(int id)
        {
            var result = await _companyServices.UnregisterCompanyAsync(id);

            int[] arr = {1,2,3};

            arr[5] = 20;

            if (result.IsSuccess)
            {
                return Ok();
            }

            StringBuilder errorMessage = new();

            foreach (var error in result.Errors)
            {
                errorMessage.Append($"- {error.Message}\n");
            }

            return BadRequest(errorMessage.ToString());
        }
    }
}
