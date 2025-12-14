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

        if(!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        Response.Cookies.Append(
            "refreshToken",
            result.Value.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth/refresh",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

        return CreatedAtAction(nameof(UsersController.GetUser), "Users", new { id = result.Value.UserData.Id },
            new AuthResponse()
            {
                UserData = result.Value.UserData,
                Token = result.Value.Token,
            });

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

        if(!result.IsSuccess)
        {
            return Unauthorized();
        }

        Response.Cookies.Append(
            "refreshToken",
            result.Value.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth/refresh",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

        return Ok(new AuthResponse()
        {
            UserData = result.Value.UserData,
            Token = result.Value.Token
        });
    }

    // POST: api/auth/refresh
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out string? refreshToken))
        {
            return Unauthorized();
        }

        Result<RefreshTokenResult> result = await mediator.Send(
            new RefreshTokenCommand(refreshToken),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return Unauthorized();
        }

        // Rotate refresh token
        Response.Cookies.Append(
            "refreshToken",
            result.Value.NewRefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth/refresh",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

        return Ok(new RefreshTokenResponse(Token: result.Value.AccessToken, RefreshToken: result.Value.NewRefreshToken));
    }


    // GET: api/auth/google-signin
    [HttpGet("google-signin")]
    public async Task<IActionResult> GoogleSignIn(string code, CancellationToken cancellationToken)
    {
        Result<LoginDTO> result = await mediator.Send(new GoogleSignInCommand(code), cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        return Ok(new AuthResponse()
        {
            UserData = result.Value.UserData,
            Token = result.Value.Token,
        });
    }
}
