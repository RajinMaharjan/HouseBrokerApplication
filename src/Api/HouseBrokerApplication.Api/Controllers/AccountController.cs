using HouseBrokerApplication.Application.DTOs;
using HouseBrokerApplication.Application.Interface;
using HouseBrokerApplication.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HouseBrokerApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var result = await _accountService.RegisterAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var (success, role, token, expiry) = await _accountService.LoginAsync(model);
            if (!success) return Unauthorized();

            return Ok(new
            {
                Message = "Login successful",
                Role = role,
                Token = token,
                ExpiresAt = expiry
            });
        }
    }

}
