using Microsoft.AspNetCore.Mvc;
using SystemPortal.Services.services.OtpServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemPortal.Api.Controllers
{
    [Route("v1/api/system-portal/otp")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        IOtpServices _otpServices;

        public OtpController(IOtpServices otpServices)
        {
            _otpServices = otpServices;
        }

        // GET: api/<OtpController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_otpServices.GetOTP());
        }
    }
}
