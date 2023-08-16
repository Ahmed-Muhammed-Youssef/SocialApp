using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public AccountController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        // POST: api/account/register
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
            var userData = await unitOfWork.UsersRepository.GetUserDTOByUsernameAsync(accountDTO.UserName);
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
        // POST: api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO loginCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(loginCredentials);
            }
            var user = await userManager.Users.Include(u => u.Pictures).FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);           
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
            var userData = await unitOfWork.UsersRepository.GetUserDTOByEmailAsync(loginCredentials.Email);

            return Ok(new TokenDTO()
            {
                UserData = userData,
                Token = await tokenService.CreateTokenAsync(user)
            });
        }

        // Utility Methods
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
