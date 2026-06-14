using HospitalManagementSystem.Models.DTO.Logins;
using HospitalManagementSystem.Services.Accounts;
using HospitalManagementSystem.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HospitalManagementSystems.API.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly JwtValidationService _jwtValidationService;

        public LoginController(ILoginRepository loginRepository, IConfiguration config)
        {
            _loginRepository = loginRepository;
            _jwtValidationService = new JwtValidationService(config);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<object> UserLogin([FromBody] LoginDto loginDto)
        {
            return await _loginRepository.LoginAsyn(loginDto);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ValidateToken")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            var result = _jwtValidationService.ValidateToken(token);
            if (result.IsValid)
            {
                return Ok(new { Valid = true, Claims = result.Principal?.Claims.Select(c => new { c.Type, c.Value }) });
            }
            else
            {
                return BadRequest(new { Valid = false, Error = result.Error });
            }
        }
    }
}
