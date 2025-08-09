using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using SystemPortal.Data.Dtos.CompanyDtos;
using SystemPortal.Data.Entities;
using SystemPortal.Services.services.CompanyServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemPortal.Api.Controllers
{
    [Route("v1/api/system-portal/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        ICompanyServices _companyServices;
        public CompanyController(ICompanyServices companyServices)
        {
            _companyServices = companyServices;
        }

        [HttpGet("all-companies")]
        public async ValueTask<ActionResult> GetCompanies()
        {
            return Ok(await _companyServices.GetCompaniesAsync());
        }

        [HttpGet("{id}")]
        public async ValueTask<ActionResult> GetCompany(int id)
        {
            Result<Company> result = await _companyServices.GetCompanyInfo(id);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound();
        }

        [HttpPost("new")]
        public async ValueTask<ActionResult> Post([FromForm] CompanyDto company)
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

        // PUT api/<CompanyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpGet("logo/{id}")]
        public async ValueTask<ActionResult> DownloadLogo(int id)
        {
            Result<(string?, string?, string?)> result = await _companyServices.GetCompanyLogoAsync(id);

            if (result.IsSuccess)
            {
                if (!System.IO.File.Exists(result.Value.Item1))
                    return NotFound();

                var fileBytes = System.IO.File.ReadAllBytes(result.Value.Item1);
                var cType = result.Value.Item2;

                return File(fileBytes, cType !, result.Value.Item3);
            }
            return NotFound();
        }

        // DELETE api/<CompanyController>/5
        [HttpDelete("{id}")]
        public async ValueTask<ActionResult> Delete(int id)
        {
            bool result = await _companyServices.UnregisterCompanyAsync(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
