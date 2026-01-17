using System.Net;
using Infrastructure.Auth.External;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Infrastructure.Test.Auth.External;

public class GoogleAuthServiceTests
{
    private static IConfiguration BuildFakeConfig()
    {
        var config = Substitute.For<IConfiguration>();
        config["Authentication:Google:ClientId"].Returns("client_id");
        config["Authentication:Google:ClientSecret"].Returns("client_secret");
        config["Authentication:Google:RedirectUri"].Returns("https://localhost/signin-google");
        config["Authentication:Google:GoogleAuthorizationEndpoint"].Returns("https://accounts.google.com/o/oauth2/v2/auth");
        config["Authentication:Google:Scope"].Returns("profile email");
        config["Authentication:Google:TokenEndpoint"].Returns("https://oauth2.googleapis.com/token");
        return config;
    }

    [Fact]
    public async Task GetUserFromGoogleAsync_ReturnsUser_WhenRequestsSucceed()
    {
        // Arrange
        const string code = "auth_code";
        const string accessToken = "access_token_123";
        const string userInfoEndpoint = "https://www.googleapis.com/userinfo/v2/me";

        var mockHttp = new MockHttpMessageHandler();

        // Mock token endpoint
        mockHttp.When("https://oauth2.googleapis.com/token")
            .Respond("application/json", JsonConvert.SerializeObject(new { access_token = accessToken }));

        // Mock user info endpoint
        mockHttp.When($"{userInfoEndpoint}?access_token={accessToken}")
            .Respond("application/json", JsonConvert.SerializeObject(new
            {
                name = "John Doe",
                email = "john@example.com",
                verified_email = true,
                picture = "https://example.com/pic.jpg"
            }));

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("https://www.googleapis.com/");

        var httpFactory = Substitute.For<IHttpClientFactory>();
        httpFactory.CreateClient("GoogleAuth").Returns(client);

        var service = new GoogleAuthService(BuildFakeConfig(), httpFactory);

        // Act
        var result = await service.GetUserFromGoogleAsync(code);

        // Assert
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john@example.com", result.Email);
        Assert.True(result.VerifiedEmail);
        Assert.Equal("https://example.com/pic.jpg", result.PictureUrl);
    }

    [Fact]
    public async Task GetUserFromGoogleAsync_Throws_WhenTokenRequestFails()
    {
        // Arrange
        const string code = "auth_code";
        var mockHttp = new MockHttpMessageHandler();

        // Simulate failed token response
        mockHttp.When("https://oauth2.googleapis.com/token")
            .Respond(HttpStatusCode.BadRequest);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("https://www.googleapis.com/");

        var httpFactory = Substitute.For<IHttpClientFactory>();
        httpFactory.CreateClient("GoogleAuth").Returns(client);

        var service = new GoogleAuthService(BuildFakeConfig(), httpFactory);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => service.GetUserFromGoogleAsync(code));
        Assert.Equal("Failed to exchange code for tokens", ex.Message);
    }

    [Fact]
    public async Task GetUserFromGoogleAsync_Throws_WhenUserInfoRequestFails()
    {
        // Arrange
        const string code = "auth_code";
        const string accessToken = "access_token_123";

        var mockHttp = new MockHttpMessageHandler();

        // Token response is OK
        mockHttp.When("https://oauth2.googleapis.com/token")
            .Respond("application/json", JsonConvert.SerializeObject(new { access_token = accessToken }));

        // User info request fails
        mockHttp.When($"https://www.googleapis.com/userinfo/v2/me?access_token={accessToken}")
            .Respond(HttpStatusCode.InternalServerError);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("https://www.googleapis.com/");

        var httpFactory = Substitute.For<IHttpClientFactory>();
        httpFactory.CreateClient("GoogleAuth").Returns(client);

        var service = new GoogleAuthService(BuildFakeConfig(), httpFactory);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => service.GetUserFromGoogleAsync(code));
        Assert.Equal("Failed to retrieve user information", ex.Message);
    }
}
