using API.Controllers.Account.Requests;
using API.Controllers.Account.Responses;

namespace API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
public class AccountController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService, IMapper mapper) : ControllerBase
{
    // POST: api/account/register
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest registerRequest)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await userManager.Users.AnyAsync(u => u.Email == registerRequest.Email))
        {
            return BadRequest("The Email is already taken.");
        }

        IdentityUser newIdentityUser = new()
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email
        };

        // Create Identity User
        var result = await userManager.CreateAsync(newIdentityUser, registerRequest.Password);

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

        ApplicationUser newApplicationUser = new()
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Sex = registerRequest.Sex,
            DateOfBirth = registerRequest.DateOfBirth,
            CityId = registerRequest.CityId,
            IdentityId = newIdentityUser.Id
        };
        
        try
        {
            await unitOfWork.ApplicationUserRepository.AddAsync(newApplicationUser);

            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await userManager.DeleteAsync(newIdentityUser);

            return BadRequest("Failed to register user");
        }

        UserDTO userData = mapper.Map<UserDTO>(newApplicationUser);

        return CreatedAtAction("Register", new { email = registerRequest.Email },
            new AuthResponse()
            {
                UserData = userData,
                Token = await tokenService.CreateTokenAsync(newIdentityUser, userData.Id)
            });
    }

    // POST: api/account/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(loginRequest);
        }
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager
            .CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            return Unauthorized();
        }

        UserDTO userData = await unitOfWork.ApplicationUserRepository.GetDtoByIdentityId(user.Id);

        return Ok(new AuthResponse()
        {
            UserData = userData,
            Token = await tokenService.CreateTokenAsync(user, userData.Id)
        });
    }
}
