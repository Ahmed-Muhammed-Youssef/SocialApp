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

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
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
            using var hasher = new HMACSHA512();
            AppUser newUser = new AppUser()
            {
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
            await context.SaveChangesAsync();
            return CreatedAtAction("Register", new { email = accountDTO.Email }, 
                new TokenDTO()
                {
                    Email = newUser.Email,
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
            return Ok(new TokenDTO()
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user)
            });
        }
        private async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
