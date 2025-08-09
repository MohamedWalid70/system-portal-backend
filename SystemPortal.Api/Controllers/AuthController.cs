using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SystemPortal.Data.Dtos.CompanyDtos;
using SystemPortal.Services.services.CompanyServices;
using SystemPortal.Services.Services.AuthServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemPortal.Api.Controllers
{
    [Route("v1/api/system-portal/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        // POST api/<AuthController>
        [HttpPost]
        public async ValueTask<ActionResult> Post([FromBody] CompanyLoginDto companyLoginDto)
        {
            Result<OutputCompanyDto> result = await _authServices.CompanyLoginAsync(companyLoginDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest();
        }
    }
}
