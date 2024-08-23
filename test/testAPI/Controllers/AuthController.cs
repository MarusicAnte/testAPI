using eStudent.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.DTO.LoginDto;
using testAPI.Services;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;

        public AuthController(IUserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.ValidateUserAsync(loginDto.Email, loginDto.Password);

            if(user is null)
                return Unauthorized();

            
            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Logout successful.");
        }
    }
}
