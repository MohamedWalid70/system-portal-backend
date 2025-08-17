using FluentResults;
using Microsoft.AspNetCore.Mvc;
using SystemPortal.Services.Services.AuthServices;
using SystemPortal.Services.Services.AuthServices.Dtos;
using SystemPortal.Services.Services.CompanyServices.Dtos;
using SystemPortal.Services.Services.UserServices.Dtos;

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
        [HttpPost("company")]
        public async ValueTask<ActionResult> CompanyLogin([FromBody] LoginDto companyLoginDto)
        {
            Result<CompanyOutputDto> result = await _authServices.CompanyLoginAsync(companyLoginDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return Unauthorized();
        }

        [HttpPost("admin")]
        public async ValueTask<ActionResult> AdminLogin([FromBody] LoginDto adminLoginDto)
        {
            Result<UserOutputDto> result = await _authServices.AdminLoginAsync(adminLoginDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return Unauthorized();
        }
    }
}
