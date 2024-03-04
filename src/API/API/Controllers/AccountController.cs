using API.Domain.Entities;
using API.Filters;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using API.Application.Interfaces.Services;
using API.Application.Interfaces;
using API.Application.DTOs.User;
using API.Application.DTOs.Registeration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class AccountController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper) : ControllerBase
    {
        // POST: api/account/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO accountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool emailExists = await userManager.Users.AnyAsync(u => u.Email == accountDTO.Email);
            bool userNameExists = await userManager.Users.AnyAsync(u => u.UserName == accountDTO.UserName);
            if (emailExists)
            {
                return BadRequest("The Email is already taken.");
            }
            if (userNameExists)
            {
                return BadRequest("The Username is already taken.");
            }
            AppUser newUser = new();
            mapper.Map(accountDTO, newUser);
            newUser.UserName = newUser.UserName.ToLower();
            var result = await userManager.CreateAsync(newUser, accountDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to register the user.");
            }
            var adddRoleresult = await userManager.AddToRoleAsync(newUser, "user");
            if (!adddRoleresult.Succeeded)
            {
                return BadRequest();
            }
            var userData = await unitOfWork.UserRepository.GetUserDTOByIdAsync(newUser.Id);
            return CreatedAtAction("Register", new { email = accountDTO.Email },
                new TokenDTO()
                {
                    UserData = userData,
                    Token = await tokenService.CreateTokenAsync(newUser)
                });
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO loginCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(loginCredentials);
            }
            var user = await userManager.Users.Include(u => u.Pictures).FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);
            if (user == null)
            {
                return Unauthorized();
            }
            var signInResult = await signInManager
                .CheckPasswordSignInAsync(user, loginCredentials.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return Unauthorized();
            }
            // var userData = await _unitOfWork.UserRepository.GetUserDTOByEmailAsync(loginCredentials.Email);

            return Ok(new TokenDTO()
            {
                UserData = mapper.Map<AppUser, UserDTO>(user),
                Token = await tokenService.CreateTokenAsync(user)
            });
        }
    }
}
