using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Orchestration.UserProvisioning;

public class UserProvisioningService(UserManager<IdentityUser> userManager, ApplicationDatabaseContext applicationDatabaseContext, IdentityDatabaseContext identityDatabaseContext) : IUserProvisioningService
{
    // <inheritdoc />
    public Task<UserProvisioningResult> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        return CreateUserAsync(email, true, firstName, lastName, null, Gender.NotSpecified, DateTime.UtcNow.AddYears(-20), 1, cancellationToken);
    }

    // <inheritdoc />
    public Task<UserProvisioningResult> CreateUserAsync(string email, bool emailVerified, string firstName, string lastName, string? password, Gender gender, DateTime dateOfBirth, int cityId,  CancellationToken cancellationToken = default)
    {
        IdentityUser identityUser = new()
        {
            Email = email,
            UserName = email,
            EmailConfirmed = emailVerified
        };

        ApplicationUser appUser = new(
            firstName: firstName,
            lastName: lastName,
            dateOfBirth: dateOfBirth,
            gender: gender,
            cityId: cityId);

        return CreateUserAsync(identityUser, appUser, [RolesNameValues.User], password, cancellationToken);
    }

    // <inheritdoc />
    public async Task<UserProvisioningResult> CreateUserAsync(IdentityUser identityUser, ApplicationUser appUser, IReadOnlyList<string> roles, string? password, CancellationToken cancellationToken = default)
    {
        using var transaction = await identityDatabaseContext.Database.BeginTransactionAsync(cancellationToken);

        applicationDatabaseContext.Database.SetDbConnection(identityDatabaseContext.Database.GetDbConnection());
        await applicationDatabaseContext.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken: cancellationToken);

        try
        {
            identityDatabaseContext.Database.UseTransaction(transaction.GetDbTransaction());

            IdentityResult result = password != null ? await userManager.CreateAsync(identityUser, password)
                : await userManager.CreateAsync(identityUser);

            if (!result.Succeeded)
                throw new InvalidOperationException("Identity creation failed.");

            foreach (var role in roles)
            {
                var adddRoleresult = await userManager.AddToRoleAsync(identityUser, role);

                if (!adddRoleresult.Succeeded)
                    throw new InvalidOperationException("Identity creation failed.");
            }

            appUser.AssociateWithIdentity(identityUser.Id);

            applicationDatabaseContext.ApplicationUsers.Add(appUser);

            await applicationDatabaseContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new UserProvisioningResult(identityUser, appUser);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

    }
}