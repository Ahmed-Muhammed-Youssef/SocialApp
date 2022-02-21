using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;

        public AccountController(DataContext context, ITokenService tokenService, IUserRepository userRepository)
        {
            this.context = context;
            this.tokenService = tokenService;
            this.userRepository = userRepository;
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
            using var hasher = new HMACSHA512();
            AppUser newUser = new AppUser()
            {
                UserName = accountDTO.UserName,
                FirstName = accountDTO.FirstName,
                LastName = accountDTO.LastName,
                Email = accountDTO.Email,
                Sex = accountDTO.Sex,
                Interest = accountDTO.Interest,
                Bio = accountDTO.Bio,
                DateOfBirth = accountDTO.DateOfBirth,
                City = accountDTO.City,
                Country = accountDTO.Country,
                Password = hasher.ComputeHash(Encoding.UTF8.GetBytes(accountDTO.Password)),
                PasswordSalt = hasher.Key
            };
            context.Users.Add(newUser);
            var userData = await userRepository.GetUserDTOByEmailAsync(accountDTO.Email);
            await context.SaveChangesAsync();
            return CreatedAtAction("Register", new {email = accountDTO.Email }, 
                new TokenDTO()
                {
                    UserData = userData,
                    Token = tokenService.CreateToken(newUser)
                });
        }
        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO loginCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(loginCredentials);
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);
           
            if(user == null)
            {
                return Unauthorized(loginCredentials);
            }

            using var hasher = new HMACSHA512(user.PasswordSalt);
            var hashedPassword = hasher.ComputeHash(Encoding.UTF8.GetBytes(loginCredentials.Password));
            for (int i = 0; i < hashedPassword.Length; i++)
            {
                if (user.Password[i] != hashedPassword[i])
                {
                    return Unauthorized(loginCredentials);
                }
            }
            var userData = await userRepository.GetUserDTOByEmailAsync(loginCredentials.Email);

            return Ok(new TokenDTO()
            {
                UserData = userData,
                Token = tokenService.CreateToken(user)
            });
        }
        private async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }
        private async Task<bool> UsernameExists(string username)
        {
            return await context.Users.AnyAsync(u => u.UserName == username);
        }
    }
}
