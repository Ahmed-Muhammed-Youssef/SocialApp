namespace API.Features.Auth;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerRequest">The user registration details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The registered user data and authentication token.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
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

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        Response.AppendRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiresAtUtc);

        return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = result.Value.UserData.Id },
            new AuthResponse()
            {
                UserData = result.Value.UserData,
                Token = result.Value.Token,
            });

    }

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="loginRequest">The login credentials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user data and authentication token.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(loginRequest);
        }

        Result<LoginDTO> result = await mediator.Send(new LoginCommand(loginRequest.Email, loginRequest.Password), cancellationToken);

        if (!result.IsSuccess)
        {
            return Unauthorized();
        }

        Response.AppendRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiresAtUtc);

        return Ok(new AuthResponse()
        {
            UserData = result.Value.UserData,
            Token = result.Value.Token
        });
    }

    /// <summary>
    /// Refreshes an access token using a refresh token cookie.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A new access token and refresh token.</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RefreshTokenResponse>> Refresh(CancellationToken cancellationToken)
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

        Response.AppendRefreshTokenCookie(result.Value.NewRefreshToken, result.Value.RefreshTokenExpiresAtUtc);

        return Ok(new RefreshTokenResponse(Token: result.Value.AccessToken, RefreshToken: result.Value.NewRefreshToken));
    }

    /// <summary>
    /// Authenticates a user using Google OAuth.
    /// </summary>
    /// <param name="googleSignInRequest">The Google sign-in request containing the credential.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user data and authentication token.</returns>
    [HttpPost("google-signin")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> GoogleSignIn(GoogleSignInRequest googleSignInRequest, CancellationToken cancellationToken)
    {
        Result<LoginDTO> result = await mediator.Send(new GoogleSignInCommand(googleSignInRequest.Credential), cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        Response.AppendRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiresAtUtc);

        return Ok(new AuthResponse()
        {
            UserData = result.Value.UserData,
            Token = result.Value.Token,
        });
    }
}
