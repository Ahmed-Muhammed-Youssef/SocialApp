using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using API.Features.Auth.Requests;
using API.Features.Auth.Responses;
using Application.Features.Auth;
using Application.Features.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;

namespace API.Test.Infrastructure;

public abstract class IntegrationTestFixture : IClassFixture<WebAppFactory>
{
    protected readonly WebAppFactory WebAppFactory;

    protected IntegrationTestFixture(WebAppFactory webAppFactory)
    {
        WebAppFactory = webAppFactory;
    }

    public HttpClient CreateClient() => WebAppFactory.CreateClient();
    private HttpClient? authenticatedClient;
    private UserDTO? authenticatedUserData;

    public WireMockServer GetWireMockServer() => WebAppFactory.GetWireMockServer();

    public async Task<HttpClient> CreateAuthenticatedClientAsync(string email = "test@test.com", string password = "Password123!")
    {
        if(authenticatedClient is not null)
        {
            return authenticatedClient;
        }

        HttpClient client = CreateClient();

        bool userExists;

        using IServiceScope scope = WebAppFactory.Services.CreateScope();
        using ApplicationDatabaseContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
        userExists = await dbContext.Users.AnyAsync(u => u.Email == email);

        if (!userExists)
        {
            RegisterRequest registerRequest = new()
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Domain.ApplicationUserAggregate.Gender.Male,
                Email = email,
                Password = password,
                DateOfBirth = new DateTime(1990, 1, 1),
                CityId = 1
            };

            var response = await client.PostAsJsonAsync(Routes.Auth.Register, registerRequest);

            response.EnsureSuccessStatusCode();

        }

        // login
        HttpResponseMessage? loginResponse = await client.PostAsJsonAsync(Routes.Auth.Login, new LoginRequest()
        {
            Email = email,
            Password = password
        });

        loginResponse.EnsureSuccessStatusCode();

        AuthResponse? authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        if(authResponse?.Token is null || authResponse.UserData is null)
        {
            throw new InvalidOperationException("Authentication failed.");
        }

        authenticatedUserData = authResponse.UserData;
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Token);

        authenticatedClient = client;

        return client;
    }

    public async Task<UserDTO> GetAuthenticatedUserDataAsync()
    {
        if (authenticatedUserData is not null)
        {
            return authenticatedUserData;
        }

        await CreateAuthenticatedClientAsync();

        return authenticatedUserData;
    }

    /// <summary>
    /// Gets the mock Google credential validator from the factory.
    /// Use this to register test credentials in your tests.
    /// </summary>
    public MockGoogleCredentialValidator GetMockGoogleValidator()
    {
        return WebAppFactory.GetMockGoogleValidator();
    }

    /// <summary>
    /// Creates an HttpClient and authenticates it via Google Sign In with a new user.
    /// </summary>
    public async Task<HttpClient> CreateAuthenticatedClientViaGoogleAsync(string email = "google.test@example.com", string givenName = "Google", string familyName = "User", string subject = "google-123")
    {
        HttpClient client = CreateClient();

        // Get the mock validator and register a test credential
        var mockValidator = GetMockGoogleValidator();
        var testCredential = mockValidator.CreateAndRegisterCredential(email, subject, givenName, familyName);

        // Call Google Sign In
        var response = await client.PostAsJsonAsync(Routes.Auth.GoogleSignIn, new { credential = testCredential });

        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>()
            ?? throw new InvalidOperationException("Failed to read auth response");

        authenticatedUserData = authResponse.UserData;

        // Extract and set the refresh token cookie
        var setCookieHeader = response.Headers
            .GetValues("Set-Cookie")
            .First(x => x.StartsWith("refreshToken=", StringComparison.OrdinalIgnoreCase));

        var refreshToken = setCookieHeader
            .Split(';')[0]
            .Split('=')[1];

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Token);
        authenticatedClient = client;

        return client;
    }
}
