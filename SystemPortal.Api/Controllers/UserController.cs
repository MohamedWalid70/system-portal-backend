using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using SystemPortal.Services.Services.CompanyServices.Dtos;
using SystemPortal.Services.Services.UserServices;
using SystemPortal.Services.Services.UserServices.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemPortal.Api.Controllers
{
    [Route("v1/api/system-portal/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IAsyncEnumerable<UserOutputDto> GetAll()
        {
            return _userServices.GetUsersAsync();

        }

        [HttpGet("all-company-employees/{id:int}")]
        public IAsyncEnumerable<UserOutputDto> GetAllCompanyEmployees(int id)
        {
            return _userServices.GetUsersByCompanyId(id);

        }

        //[Authorize(Roles = "Admin,Company")]
        [HttpGet("all-users-except-companies")]
        public async Task<IEnumerable<UserOutputDto>> GetUsers()
        {
            return await _userServices.GetUsersExceptCompaniesAsync();

        }

        //[Authorize(Roles = "Admin,Company")]
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> GetUser(int id)
        {
            var result = await _userServices.GetUserAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound();
        }

        //[Authorize(Roles = "Admin,Company")]
        [HttpPost("new")]
        public async ValueTask<ActionResult> AddUser(UserSignUpDto user)
        {

            Result result = await _userServices.AddEmployeeAsync(user);

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

        //[Authorize(Roles = "Admin,Company")]
        [HttpPatch]
        public async ValueTask<ActionResult> UpdateUser(UserUpdateDto user)
        {
            var result = await _userServices.UpdateUserAsync(user);

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

        //[Authorize(Roles = "Admin,Company")]
        [HttpDelete("{id}")]
        public async ValueTask<ActionResult> RemoveUser(int id)
        {
            var result = await _userServices.RemoveEmployeeAsync(id);

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
