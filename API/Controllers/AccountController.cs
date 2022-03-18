using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IUserRepository userRepository, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO accountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(await EmailExists(accountDTO.Email))
            {
                return BadRequest("The Email is already taken.");
            }
            if(await UsernameExists(accountDTO.UserName))
            {
                return BadRequest("The Username is already taken.");
            }
            AppUser newUser = new AppUser();
            mapper.Map(accountDTO, newUser);
            newUser.UserName = newUser.UserName.ToLower();
            var result = await userManager.CreateAsync(newUser, accountDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to register the user.");
            }
            var userData = await userRepository.GetUserDTOByUsernameAsync(accountDTO.UserName);
            var adddRoleresult = await userManager.AddToRoleAsync(newUser, "user");
            if (!adddRoleresult.Succeeded)
            {
                return BadRequest();
            }
            return CreatedAtAction("Register", new {email = accountDTO.Email }, 
                new TokenDTO()
                {
                    UserData = userData,
                    Token = await tokenService.CreateTokenAsync(newUser)
                });
        }
        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO loginCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(loginCredentials);
            }
            var user = await userManager.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);           
            if(user == null)
            {
                return Unauthorized();
            }
            var signInResult = await signInManager
                .CheckPasswordSignInAsync(user, loginCredentials.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return Unauthorized();
            }
            var userData = await userRepository.GetUserDTOByEmailAsync(loginCredentials.Email);

            return Ok(new TokenDTO()
            {
                UserData = userData,
                Token = await tokenService.CreateTokenAsync(user)
            });
        }
        private async Task<bool> EmailExists(string email)
        {
            return await userManager.Users.AnyAsync(u => u.Email == email);
        }
        private async Task<bool> UsernameExists(string username)
        {
            return await userManager.Users.AnyAsync(u => u.UserName == username);
        }
    }
}
