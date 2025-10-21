using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace MVC.Factories;

public class ApplicationUserClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>
where TUser : IdentityUser
where TRole : IdentityRole
{
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationUserClaimsPrincipalFactory(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IOptions<IdentityOptions> options, IUnitOfWork unitOfWork) : base(userManager, roleManager, options)
    {
        _unitOfWork = unitOfWork;
    }
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        Domain.Entities.ApplicationUser applicationUser = await _unitOfWork.ApplicationUserRepository.GetByIdentity(user.Id) ?? throw new InvalidOperationException("Failed to get user.");

        Claim? defaultClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
        
        if (defaultClaim != null)
        {
            identity.RemoveClaim(defaultClaim);
        }

        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()));

        return identity;
    }
}
