using Application.DTOs.User;
using Application.Interfaces;
using Application.Interfaces.Services;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Application.Features.Account.Login;

public class LoginHandler(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService) 
    : ICommandHandler<LoginCommand, Result<LoginDTO>>
{
    public async ValueTask<Result<LoginDTO>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        IdentityUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken: cancellationToken);

        if (user == null)
        {
            return Result<LoginDTO>.Unauthorized();
        }

        SignInResult signInResult = await signInManager
            .CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            return Result<LoginDTO>.Unauthorized();
        }

        UserDTO? userData = await unitOfWork.ApplicationUserRepository.GetDtoByIdentityId(user.Id);

        if(userData is null)
        {
            return Result<LoginDTO>.Unauthorized();
        }

        string token = await tokenService.CreateTokenAsync(user, userData.Id);

        return Result<LoginDTO>.Success(new LoginDTO(userData, token));
    }
}
