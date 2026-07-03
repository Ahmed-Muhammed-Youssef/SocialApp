using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using API.Features.Auth.Requests;
using API.Features.Auth.Responses;
using API.Test.Infrastructure;

namespace API.Test.Features;

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
}
