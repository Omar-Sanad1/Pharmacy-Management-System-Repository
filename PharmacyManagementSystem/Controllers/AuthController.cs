using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomerAsync([FromBody]RegisterCustomerModel registerCustomer)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var registeredCustomer = await _authService.RegisterCustomerAsync(registerCustomer);
            return Ok(registeredCustomer);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpPost("RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployeeAsync([FromBody]RegisterEmployeeModel registerEmployee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registeredEmployee = await _authService.RegisterEmployeeAsync(registerEmployee);
            return Ok(registeredEmployee);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var LoginedUser = await _authService.LoginAsync(loginModel);
            return Ok(LoginedUser);
        }
    }
}
