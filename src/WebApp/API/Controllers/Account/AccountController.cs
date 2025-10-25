namespace API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
public class AccountController(IMediator mediator) : ControllerBase
{
    // POST: api/account/register
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
