using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using API.Application.Interfaces.Services;
using API.Filters;
using API.Application.Authentication.Google;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class AccountExternalController(IGoogleAuthService _googleAuthService, UserManager<AppUser> userManager, ITokenService tokenService) : ControllerBase
    {
        // GET: api/AccountExternal/login-google
        [HttpGet("login-google")]
        public IActionResult GoogleSignIn()
        {
            return Redirect(_googleAuthService.BuildGoogleSignInUrl());
        }

        // GET: api/AccountExternal/callback-google
        [HttpGet("callback-google")]
        public async Task<IActionResult> GoogleCallback(string code)
        {
            var userInfo = await _googleAuthService.GetUserFromGoogleAsync(code);
            return Ok(userInfo); 
        }
    }
}
