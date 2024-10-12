using Domain.Entities;
using API.Filters;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Application.Interfaces;
using Application.DTOs.User;
using Application.DTOs.Registeration;
using Domain.Constants;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class AccountController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService, IMapper mapper) : ControllerBase
    {
        // POST: api/account/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO accountDTO)
        {

            // throw new NotImplementedException();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await userManager.Users.AnyAsync(u => u.Email == accountDTO.Email))
            {
                return BadRequest("The Email is already taken.");
            }

            IdentityUser newIdentityUser = new()
            {
                UserName = accountDTO.Email,
                Email = accountDTO.Email
            };

            

            // Create Identity User

            var result = await userManager.CreateAsync(newIdentityUser, accountDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to register the user.");
            }

            var adddRoleresult = await userManager.AddToRoleAsync(newIdentityUser, RolesNameValues.User);

            if (!adddRoleresult.Succeeded)
            {
                return BadRequest();
            }

            // Create application user

            ApplicationUser newApplicationUser = new();

            mapper.Map(accountDTO, newApplicationUser);

            newApplicationUser.IdentityId = newIdentityUser.Id;

            await unitOfWork.ApplicationUserRepository.AddApplicationUser(newApplicationUser);

            if (!await unitOfWork.SaveChangesAsync())
            {
                return BadRequest();
            }

            UserDTO userData = mapper.Map<UserDTO>(newApplicationUser);

            userData.Username = accountDTO.Email;

            return CreatedAtAction("Register", new { email = accountDTO.Email },
                new TokenDTO()
                {
                    UserData = userData,
                    Token = await tokenService.CreateTokenAsync(newIdentityUser)
                });
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO loginCredentials)
        {
            throw new NotImplementedException();
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(loginCredentials);
            //}
            //var user = await userManager.Users.Include(u => u.Pictures).FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);
            //if (user == null)
            //{
            //    return Unauthorized();
            //}
            //var signInResult = await signInManager
            //    .CheckPasswordSignInAsync(user, loginCredentials.Password, lockoutOnFailure: false);
            //if (!signInResult.Succeeded)
            //{
            //    return Unauthorized();
            //}
            //// var userData = await _unitOfWork.UserRepository.GetUserDTOByEmailAsync(loginCredentials.Email);

            //return Ok(new TokenDTO()
            //{
            //    UserData = mapper.Map<AppUser, UserDTO>(user),
            //    Token = await tokenService.CreateTokenAsync(user)
            //});
        }
    }
}
