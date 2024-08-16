using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces.Services;
using API.Filters;
using Application.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.DTOs.Registeration;
using Application.DTOs.User;
using AutoMapper;
using Domain;
using Domain.Constants;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class AccountExternalController(IGoogleAuthService _googleAuthService, UserManager<AppUser> userManager, ITokenService tokenService, IMapper _mapper, PasswordGenerationService _passwordGenerationService) : ControllerBase
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
               databaseUser = new() 
               { 
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName ?? "",
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    EmailConfirmed = userInfo.EmailConfirmed,
                    Sex = ' ',
                    Interest = 'b',
                    Country = "",
                    City = ""
                    
               };
                var password = _passwordGenerationService.GenerateRandomPassword();
                var result = await userManager.CreateAsync(databaseUser, password);
                if (!result.Succeeded)
                {
                    return BadRequest("Failed to register the user.");
                }
                var adddRoleresult = await userManager.AddToRoleAsync(databaseUser, RolesNameValues.User);
                if (!adddRoleresult.Succeeded)
                {
                    return BadRequest();
                }
            }

            return Ok(new TokenDTO()
            {
                UserData = _mapper.Map<AppUser, UserDTO>(databaseUser),
                Token = await tokenService.CreateTokenAsync(databaseUser)
            });
        }
    }
}
