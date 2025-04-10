﻿namespace API.Controllers
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
                await userManager.DeleteAsync(newIdentityUser);

                return BadRequest();
            }

            // Create application user

            ApplicationUser newApplicationUser = new();
            
            try
            {
                mapper.Map(accountDTO, newApplicationUser);

                newApplicationUser.IdentityId = newIdentityUser.Id;

                await unitOfWork.ApplicationUserRepository.AddAsync(newApplicationUser);

                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await userManager.DeleteAsync(newIdentityUser);

                return BadRequest("Failed to register user");
            }

            UserDTO userData = mapper.Map<UserDTO>(newApplicationUser);

            return CreatedAtAction("Register", new { email = accountDTO.Email },
                new TokenDTO()
                {
                    UserData = userData,
                    Token = await tokenService.CreateTokenAsync(newIdentityUser, userData.Id)
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
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager
                .CheckPasswordSignInAsync(user, loginCredentials.Password, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return Unauthorized();
            }

            UserDTO userData = await unitOfWork.ApplicationUserRepository.GetDtoByIdentityId(user.Id);

            return Ok(new TokenDTO()
            {
                UserData = userData,
                Token = await tokenService.CreateTokenAsync(user, userData.Id)
            });
        }
    }
}
