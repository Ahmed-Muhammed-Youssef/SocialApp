using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using API.Application.Interfaces.Services;
using API.Filters;
using API.Application.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.Application.DTOs.Registeration;
using API.Application.DTOs.User;
using AutoMapper;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class AccountExternalController(IGoogleAuthService _googleAuthService, UserManager<AppUser> userManager, ITokenService tokenService, IMapper _mapper) : ControllerBase
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

            AppUser databaseUser = await userManager.Users.Where(u => u.Email == userInfo.Email).FirstOrDefaultAsync();

            if (databaseUser is null)
            {
                // Create a new user
                // But there is a problem with the unused username?
                throw new NotImplementedException();
            }

            return Ok(new TokenDTO()
            {
                UserData = _mapper.Map<AppUser, UserDTO>(databaseUser),
                Token = await tokenService.CreateTokenAsync(databaseUser)
            });
        }
    }
}
