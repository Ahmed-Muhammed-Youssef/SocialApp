namespace Application.Common.Interfaces.Orchestration;

public sealed class UserProvisioningResult(IdentityUser identityUser, ApplicationUser applicationUser)
{
    public IdentityUser IdentityUser { get; } = identityUser;
    public ApplicationUser ApplicationUser { get; } = applicationUser;
}

