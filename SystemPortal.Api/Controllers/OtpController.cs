using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemPortal.Data.Entities;
using SystemPortal.Services.Services.OtpServices;
using SystemPortal.Services.Services.OtpServices.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemPortal.Api.Controllers
{
    [Route("v1/api/system-portal/otps")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        IOtpServices _otpServices;

        public OtpController(IOtpServices otpServices)
        {
            _otpServices = otpServices;
        }

        [HttpGet]
        public async Task<ActionResult> GetOtp()
        {
            OtpOutputDto otp = await _otpServices.GenerateOtpAsync();

            return Ok(otp);
        }

        [HttpPost("verification")]
        public async ValueTask<ActionResult> VerifyOtp(OtpInputDto inputOtp)
        {
            Result result = await _otpServices.VerifyOtp(inputOtp.Id);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest();
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IAsyncEnumerable<Otp> GetAllOtps()
        {
            var otp = _otpServices.GetOtpsAsync();

            return otp;
        }
    }
}
