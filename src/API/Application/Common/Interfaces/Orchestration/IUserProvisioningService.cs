namespace Application.Common.Interfaces.Orchestration;

public interface IUserProvisioningService
{
    /// <summary>
    /// Asynchronously creates a new identity user and application user with the specified email address and name information.
    /// </summary>
    /// <param name="email">The email address to associate with the new user. Cannot be null or empty. Must be a valid email format.</param>
    /// <param name="firstName">The first name of the user. Cannot be null or empty.</param>
    /// <param name="lastName">The last name of the user. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created ApplicationUser and IdentityUser
    /// instance.</returns>
    public Task<UserProvisioningResult> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously creates a new identity user and application user with the specified profile information and credentials.
    /// </summary>
    /// <param name="email">The email address for the new user. Cannot be null or empty.</param>
    /// <param name="emailVerified">A value indicating whether the user's email address has already been verified. Set to <see langword="true"/> if
    /// the email is verified; otherwise, <see langword="false"/>.</param>
    /// <param name="firstName">The first name of the user. Cannot be null or empty.</param>
    /// <param name="lastName">The last name of the user. Cannot be null or empty.</param>
    /// <param name="password">The password for the new user account. Can be null if the user is to be created without a password.</param>
    /// <param name="gender">The gender of the user.</param>
    /// <param name="dateOfBirth">The date of birth of the user.</param>
    /// <param name="cityId">The identifier of the city associated with the user.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created <see
    /// cref="UserProvisioningResult"/> instance.</returns>
    public Task<UserProvisioningResult> CreateUserAsync(string email, bool emailVerified, string firstName, string lastName, string? password, Gender gender, DateTime dateOfBirth, int cityId, CancellationToken cancellationToken);
}