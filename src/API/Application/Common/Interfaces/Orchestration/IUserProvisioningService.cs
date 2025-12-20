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
    Task<UserProvisioningResult> CreateUserAsync(string email, string firstName, string lastName, CancellationToken cancellationToken = default);

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
    Task<UserProvisioningResult> CreateUserAsync(string email, bool emailVerified, string firstName, string lastName, string? password, Gender gender, DateTime dateOfBirth, int cityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates a new user account using the specified identity and application user information, and
    /// optionally sets an initial password.
    /// </summary>
    /// <remarks>The method provisions both identity and application user data. If a password is provided, it
    /// will be set for the new user; otherwise, the account will be created without a password. The operation may fail
    /// if user information is invalid or if a user with the same credentials already exists.</remarks>
    /// <param name="identityUser">The identity user information to associate with the new account. Cannot be null.</param>
    /// <param name="appUser">The application-specific user details to provision for the new account. Cannot be null.</param>
    /// <param name="roles">The roles to assign to the new user account. Cannot be null.</param>
    /// <param name="password">The initial password to set for the user account, or null to create the account without a password.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the user creation operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a UserProvisioningResult indicating
    /// the outcome of the user creation process.</returns>
    Task<UserProvisioningResult> CreateUserAsync(IdentityUser identityUser, ApplicationUser appUser, IReadOnlyList<string> roles, string? password, CancellationToken cancellationToken = default);
}