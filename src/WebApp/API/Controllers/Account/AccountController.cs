using API.Controllers.Account.Requests;
using API.Controllers.Account.Responses;
using Application.Features.Account.Login;
using Mediator;
using Shared.Results;

namespace API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
public class AccountController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ITokenService tokenService, IMapper mapper, IMediator mediator) : ControllerBase
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

        Result<LoginDTO> result = await mediator.Send(new LoginCommand(loginRequest.Email, loginRequest.Password));

        if(result.IsSuccess)
        {
            return Ok(new AuthResponse()
            {
                UserData = result.Value.UserData,
                Token = result.Value.Token
            });
        }
        else
        {
            return Unauthorized();
        }
    }
}
