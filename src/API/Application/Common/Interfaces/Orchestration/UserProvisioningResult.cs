namespace Application.Common.Interfaces.Orchestration;

public sealed class UserProvisioningResult
{
    public IdentityUser IdentityUser { get; }
    public ApplicationUser ApplicationUser { get; }

    public bool IsNewIdentityUser { get; }
    public bool IsNewApplicationUser { get; }

    public UserProvisioningResult(IdentityUser identityUser, ApplicationUser applicationUser)
    {
        IdentityUser = identityUser;
        ApplicationUser = applicationUser;
    }
}

