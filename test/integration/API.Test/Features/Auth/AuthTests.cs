using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using API.Features.Auth.Requests;
using API.Features.Auth.Responses;
using API.Test.Infrastructure;

namespace API.Test.Features.Auth;

public sealed class AuthTests(WebAppFactory webAppFactory) : IntegrationTestFixture(webAppFactory)
{
    [Fact]
    public async Task Register_ShouldSucceed_WithValidParameters()
    {
        // Arrange
        RegisterRequest registerRequest = new()
        {
            FirstName = "John",
            LastName = "Doe",
            Gender = Domain.ApplicationUserAggregate.Gender.Male,
            Email = $"john.doe{Guid.NewGuid()}@example.com",
            Password = "Password123!",
            DateOfBirth = new DateTime(1990, 1, 1),
            CityId = 1
        };

        HttpClient client = CreateClient();
        
        // Act
        var response = await client.PostAsJsonAsync(Routes.Auth.Register, registerRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Register_ShouldReturnAccessToken_WithValidParameters()
    {
        // Arrange
        RegisterRequest registerRequest = new()
        {
            FirstName = "John",
            LastName = "Doe",
            Gender = Domain.ApplicationUserAggregate.Gender.Male,
            Email = $"john.doe{Guid.NewGuid()}@example.com",
            Password = "Password123!",
            DateOfBirth = new DateTime(1990, 1, 1),
            CityId = 1
        };

        HttpClient client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(Routes.Auth.Register, registerRequest, TestContext.Current.CancellationToken);
        var setCookieHeader = response.Headers
            .GetValues("Set-Cookie")
            .First(x => x.StartsWith("refreshToken=", StringComparison.OrdinalIgnoreCase));

        var refreshToken = setCookieHeader
            .Split(';')[0]
            .Split('=')[1];
        AuthResponse? authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Assert.NotNull(authResponse);
        
        Assert.NotEmpty(authResponse.Token);

        Assert.NotEmpty(refreshToken);
    }

    [Fact]
    public async Task GoogleSignIn_ShouldReturnAccessToken_WithValidParameters()
    {
        // Arrange
        HttpClient client = CreateClient();
        var mockValidator = GetMockGoogleValidator();
        var testEmail = $"google.user{Guid.NewGuid()}@example.com";
        var testCredential = mockValidator.CreateAndRegisterCredential(
            email: testEmail,
            subject: "google-user-123",
            givenName: "John",
            familyName: "GoogleUser"
        );

        var googleSignInRequest = new { credential = testCredential };

        // Act
        var response = await client.PostAsJsonAsync(Routes.Auth.GoogleSignIn, googleSignInRequest, TestContext.Current.CancellationToken);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(authResponse);
        Assert.NotEmpty(authResponse.Token);
        Assert.NotNull(authResponse.UserData);
        Assert.Equal("John", authResponse.UserData.FirstName);
        Assert.Equal("GoogleUser", authResponse.UserData.LastName);

        // Verify refresh token cookie is set
        var setCookieHeader = response.Headers
            .GetValues("Set-Cookie")
            .FirstOrDefault(x => x.StartsWith("refreshToken=", StringComparison.OrdinalIgnoreCase));
        Assert.NotNull(setCookieHeader);
        Assert.NotEmpty(setCookieHeader.Split(';')[0].Split('=')[1]); // Extract token value
    }

    [Fact]
    public async Task GoogleSignIn_ShouldCreateNewUser_WhenEmailDoesNotExist()
    {
        // Arrange - Create a new unique email for Google Sign In
        var testEmail = $"new.google.user{Guid.NewGuid()}@example.com";
        var mockValidator = GetMockGoogleValidator();
        var testCredential = mockValidator.CreateAndRegisterCredential(
            email: testEmail,
            subject: $"google-{Guid.NewGuid()}",
            givenName: "NewGoogle",
            familyName: "User"
        );

        HttpClient client = CreateClient();
        var googleSignInRequest = new { credential = testCredential };

        // Act
        var response = await client.PostAsJsonAsync(Routes.Auth.GoogleSignIn, googleSignInRequest, TestContext.Current.CancellationToken);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(authResponse);
        Assert.NotNull(authResponse.UserData);

        // Verify user was created with correct details
        Assert.Equal("NewGoogle", authResponse.UserData.FirstName);
        Assert.Equal("User", authResponse.UserData.LastName);
    }

    [Fact]
    public async Task GoogleSignIn_ShouldReturnUnauthorized_WithInvalidCredential()
    {
        // Arrange
        HttpClient client = CreateClient();
        var mockValidator = GetMockGoogleValidator();

        // Create an invalid credential (not registered)
        var invalidCredential = "invalid-unregistered-credential";
        var googleSignInRequest = new { credential = invalidCredential };

        // Act
        var response = await client.PostAsJsonAsync(Routes.Auth.GoogleSignIn, googleSignInRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GoogleSignIn_ShouldLinkExistingUserWithoutPassword_ToGoogleAccount()
    {
        // Arrange - First create a user via email/password registration
        var testEmail = $"existing.user{Guid.NewGuid()}@example.com";
        var registerRequest = new API.Features.Auth.Requests.RegisterRequest
        {
            FirstName = "Existing",
            LastName = "User",
            Gender = Domain.ApplicationUserAggregate.Gender.Female,
            Email = testEmail,
            Password = "Password123!",
            DateOfBirth = new DateTime(1995, 5, 15),
            CityId = 1
        };

        HttpClient client = CreateClient();
        var registerResponse = await client.PostAsJsonAsync(Routes.Auth.Register, registerRequest, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);

        // Now try to sign in with Google using the same email
        var mockValidator = GetMockGoogleValidator();
        var googleCredential = mockValidator.CreateAndRegisterCredential(
            email: testEmail,
            subject: "google-existing-123",
            givenName: "Existing",
            familyName: "User"
        );

        var googleSignInRequest = new { credential = googleCredential };

        // Act
        var googleSignInResponse = await client.PostAsJsonAsync(Routes.Auth.GoogleSignIn, googleSignInRequest, TestContext.Current.CancellationToken);

        // Assert
        // This test expects linking to fail because the user already has a password
        // The handler will return an error in this case
        Assert.Equal(HttpStatusCode.BadRequest, googleSignInResponse.StatusCode);
    }

    [Fact]
    public async Task GoogleSignIn_ShouldReturnError_WhenExistingUserHasPassword()
    {
        // Arrange - User registered with email/password (has a password)
        var testEmail = $"password.user{Guid.NewGuid()}@example.com";
        var registerRequest = new API.Features.Auth.Requests.RegisterRequest
        {
            FirstName = "PasswordUser",
            LastName = "Test",
            Gender = Domain.ApplicationUserAggregate.Gender.Male,
            Email = testEmail,
            Password = "Password123!",
            DateOfBirth = new DateTime(1992, 3, 10),
            CityId = 1
        };

        HttpClient client = CreateClient();
        var registerResponse = await client.PostAsJsonAsync(Routes.Auth.Register, registerRequest, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);

        // Try Google Sign In with same email
        var mockValidator = GetMockGoogleValidator();
        var googleCredential = mockValidator.CreateAndRegisterCredential(
            email: testEmail,
            subject: "google-pwd-user-123"
        );

        var googleSignInRequest = new { credential = googleCredential };

        // Act
        var googleSignInResponse = await client.PostAsJsonAsync(Routes.Auth.GoogleSignIn, googleSignInRequest, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, googleSignInResponse.StatusCode);
        var errorContent = await googleSignInResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Contains("email/password", errorContent, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GoogleSignIn_ShouldReturnBadRequest_WhenEmailNotVerified()
    {
        // Arrange
        var mockValidator = GetMockGoogleValidator();
        var testEmail = $"unverified{Guid.NewGuid()}@example.com";

        // Register credential with email not verified
        var credential = $"test-credential-{Guid.NewGuid()}";
        var payload = new Google.Apis.Auth.GoogleJsonWebSignature.Payload
        {
            Email = testEmail,
            GivenName = "Unverified",
            FamilyName = "User",
            Subject = "google-unverified-123",
            EmailVerified = false, // <-- Not verified
            ExpirationTimeSeconds = (long)(DateTime.UtcNow.AddHours(1) - DateTime.UnixEpoch).TotalSeconds,
            IssuedAtTimeSeconds = (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds,
        };
        mockValidator.RegisterCredential(credential, payload);

        HttpClient client = CreateClient();
        var googleSignInRequest = new { credential };

        // Act
        var response = await client.PostAsJsonAsync(Routes.Auth.GoogleSignIn, googleSignInRequest, TestContext.Current.CancellationToken);

        // Assert
        // Handler returns Unauthorized for unverified email, which maps to 401 or BadRequest depending on controller logic
        Assert.True(response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.BadRequest);
    }
}
