namespace API.Features.Auth;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
public class AuthController(IMediator mediator) : ControllerBase
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
            Gender = registerRequest.Gender,
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
                    Token = result.Value.Token,
                    RefreshToken = result.Value.RefreshToken
                });
        }
        else
        {
            return BadRequest(result.Errors);
        }

    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(loginRequest);
        }

        Result<LoginDTO> result = await mediator.Send(new LoginCommand(loginRequest.Email, loginRequest.Password), cancellationToken);

        if(result.IsSuccess)
        {
            return Ok(new AuthResponse()
            {
                UserData = result.Value.UserData,
                Token = result.Value.Token,
                RefreshToken = result.Value.RefreshToken
            });
        }
        else
        {
            return Unauthorized();
        }
    }

    // GET: api/auth/google-signin
    [HttpGet("google-signin")]
    public async Task<IActionResult> GoogleSignIn(string code, CancellationToken cancellationToken)
    {
        Result<LoginDTO> result = await mediator.Send(new GoogleSignInCommand(code), cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(new AuthResponse()
            {
                UserData = result.Value.UserData,
                Token = result.Value.Token,
                RefreshToken = result.Value.RefreshToken
            });
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
