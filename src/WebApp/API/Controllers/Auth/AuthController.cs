using API.Controllers.Auth.Requests;
using API.Controllers.Auth.Responses;
using Application.Common.Interfaces;
using Application.Features.Auth;
using Application.Features.Auth.Login;
using Application.Features.Auth.Register;

namespace API.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
public class AuthController(IMediator mediator, IGoogleAuthService _googleAuthService, UserManager<IdentityUser> userManager, ITokenService tokenService, PasswordGenerationService _passwordGenerationService, IUnitOfWork unitOfWork) : ControllerBase
{
    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        RegisterCommand registerCommand = new()
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            Password = registerRequest.Password,
            Sex = registerRequest.Sex,
            DateOfBirth = registerRequest.DateOfBirth,
            CityId = registerRequest.CityId
        };


        Result<RegisterDTO> result = await mediator.Send(registerCommand, cancellationToken);

        if(result.IsSuccess)
        {
            return CreatedAtAction(nameof(UsersController.GetUser), "Users", new { id = result.Value.UserData.Id },
                new AuthResponse()
                {
                    UserData = result.Value.UserData,
                    Token = result.Value.Token
                });
        }
        else
        {
            return BadRequest(result.Errors);
        }

    }

    // POST: api/auth/login
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

    // GET: api/AccountExternal/login-google
    [HttpGet("login-google")]
    public IActionResult GoogleSignIn()
    {
        return Redirect(_googleAuthService.BuildGoogleSignInUrl());
    }

    // GET: api/AccountExternal/callback-google
    [HttpGet("callback-google")]
    public async Task<IActionResult> GoogleCallback(string code)
    {

        GoogleUserInfo userInfo = await _googleAuthService.GetUserFromGoogleAsync(code);

        IdentityUser? identityUser = await userManager.Users.Where(u => u.Email == userInfo.Email).FirstOrDefaultAsync();

        UserDTO? userDTO = new()
        {
            FirstName = userInfo.Name,
            LastName = "",
            ProfilePictureUrl = userInfo.PictureUrl,
            Bio = ""
        };

        if (identityUser is null)
        {
            identityUser = new()
            {
                Email = userInfo.Email,
                UserName = userInfo.Email,
                EmailConfirmed = userInfo.VerifiedEmail
            };

            var password = _passwordGenerationService.GenerateRandomPassword();
            var result = await userManager.CreateAsync(identityUser, password);


            if (!result.Succeeded)
            {
                return BadRequest("Failed to register the user.");
            }
            var adddRoleresult = await userManager.AddToRoleAsync(identityUser, RolesNameValues.User);
            if (!adddRoleresult.Succeeded)
            {
                return BadRequest();
            }

            // @ToDo: add the new user data [NEED PLANNING]
        }
        else
        {
            userDTO = await unitOfWork.ApplicationUserRepository.GetDtoByIdentityId(identityUser.Id);

            if (userDTO is null)
            {
                return BadRequest("Failed to retrieve the user data.");
            }
        }

        return Ok(new AuthResponse()
        {
            UserData = userDTO,
            Token = await tokenService.CreateTokenAsync(identityUser, userDTO.Id)
        });
    }
}
