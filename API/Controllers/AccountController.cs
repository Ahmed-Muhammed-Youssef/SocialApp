using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        // POST: api/account/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO accountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await EmailExists(accountDTO.Email))
            {
                return BadRequest("The Email is already taken.");
            }
            if (await UsernameExists(accountDTO.UserName))
            {
                return BadRequest("The Username is already taken.");
            }
            AppUser newUser = new AppUser();
            _mapper.Map(accountDTO, newUser);
            newUser.UserName = newUser.UserName.ToLower();
            var result = await _userManager.CreateAsync(newUser, accountDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to register the user.");
            }
            var userData = await _unitOfWork.UserRepository.GetUserDTOByUsernameAsync(accountDTO.UserName);
            var adddRoleresult = await _userManager.AddToRoleAsync(newUser, "user");
            if (!adddRoleresult.Succeeded)
            {
                return BadRequest();
            }
            return CreatedAtAction("Register", new { email = accountDTO.Email },
                new TokenDTO()
                {
                    UserData = userData,
                    Token = await _tokenService.CreateTokenAsync(newUser)
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
            var user = await _userManager.Users.Include(u => u.Pictures).FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);
            if (user == null)
            {
                return Unauthorized();
            }
            var signInResult = await _signInManager
                .CheckPasswordSignInAsync(user, loginCredentials.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return Unauthorized();
            }
            var userData = await _unitOfWork.UserRepository.GetUserDTOByEmailAsync(loginCredentials.Email);

            return Ok(new TokenDTO()
            {
                UserData = userData,
                Token = await _tokenService.CreateTokenAsync(user)
            });
        }

        // Utility Methods
        private async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(u => u.Email == email);
        }
        private async Task<bool> UsernameExists(string username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == username);
        }
    }
}
