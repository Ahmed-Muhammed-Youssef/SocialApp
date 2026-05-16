using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using API.Features.Auth.Requests;
using API.Features.Auth.Responses;
using Application.Features.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Test.Infrastructure;

public abstract class IntegrationTestFixture(WebAppFactory webAppFactory) : IClassFixture<WebAppFactory>
{
    public HttpClient CreateClient() => webAppFactory.CreateClient();
    private HttpClient? authenticatedClient;
    private UserDTO? authenticatedUserData;

    public async Task<HttpClient> CreateAuthenticatedClientAsync(string email = "test@test.com", string password = "Password123!")
    {
        if(authenticatedClient is not null)
        {
            return authenticatedClient;
        }

        HttpClient client = CreateClient();

        bool userExists;

        using IServiceScope scope = webAppFactory.Services.CreateScope();
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
}
