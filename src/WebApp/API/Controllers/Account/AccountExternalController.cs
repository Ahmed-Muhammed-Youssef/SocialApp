using API.Controllers.Account.Responses;

namespace API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
public class AccountExternalController(IGoogleAuthService _googleAuthService, UserManager<IdentityUser> userManager, ITokenService tokenService, PasswordGenerationService _passwordGenerationService, IUnitOfWork unitOfWork) : ControllerBase
{
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

            if(userDTO is null)
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
