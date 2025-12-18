using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Orchestration.UserProvisioning;

public class UserProvisioningService(UserManager<IdentityUser> userManager, ApplicationDatabaseContext applicationDatabaseContext, IdentityDatabaseContext identityDatabaseContext) : IUserProvisioningService
{
    public Task<UserProvisioningResult> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken)
    {
        return CreateUserAsync(email, true, firstName, lastName, null, Gender.NotSpecified, DateTime.UtcNow.AddYears(-20), 1, cancellationToken);
    }

    public async Task<UserProvisioningResult> CreateUserAsync(string email, bool emailVerified, string firstName, string lastName, string? password, Gender gender, DateTime dateOfBirth, int cityId,  CancellationToken cancellationToken)
    {
        await using var transaction = await applicationDatabaseContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            identityDatabaseContext.Database.UseTransaction(transaction.GetDbTransaction());

            IdentityUser identityUser = new()
            {
                Email = email,
                UserName = email,
                EmailConfirmed = emailVerified
            };

            IdentityResult result = password != null ? await userManager.CreateAsync(identityUser, password)
                : await userManager.CreateAsync(identityUser);

            if (!result.Succeeded)
                throw new InvalidOperationException("Identity creation failed.");

            var adddRoleresult = await userManager.AddToRoleAsync(identityUser, RolesNameValues.User);

            if (!adddRoleresult.Succeeded)
                throw new InvalidOperationException("Identity creation failed.");

            ApplicationUser appUser = new(
                identityId: identityUser.Id,
                firstName: firstName,
                lastName: lastName,
                dateOfBirth: dateOfBirth,
                gender: gender,
                cityId: cityId);
        
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