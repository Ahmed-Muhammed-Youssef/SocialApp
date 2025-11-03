using Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Test.Auth;

public class CurrentUserServiceTests
{
    private static HttpContextAccessor CreateHttpContextAccessorWithClaims(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext { User = principal };

        return new HttpContextAccessor { HttpContext = context };
    }

    [Fact]
    public void GetEmail_ReturnsEmailClaim()
    {
        // Arrange
        var accessor = CreateHttpContextAccessorWithClaims(
        [
            new Claim(ClaimTypes.Email, "test@example.com")
        ]);

        var service = new CurrentUserService(accessor);

        // Act
        string email = service.GetEmail();

        // Assert
        Assert.Equal("test@example.com", email);
    }

    [Fact]
    public void GetPublicId_ReturnsParsedId()
    {
        // Arrange
        var accessor = CreateHttpContextAccessorWithClaims(
        [
            new Claim(ClaimTypes.NameIdentifier, "42")
        ]);

        var service = new CurrentUserService(accessor);

        // Act
        int id = service.GetPublicId();

        // Assert
        Assert.Equal(42, id);
    }

    [Fact]
    public void GetRoles_ReturnsAllRoles()
    {
        // Arrange
        var accessor = CreateHttpContextAccessorWithClaims(
        [
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "User")
        ]);

        var service = new CurrentUserService(accessor);

        // Act
        var roles = service.GetRoles();

        // Assert
        Assert.Contains("Admin", roles);
        Assert.Contains("User", roles);
        Assert.Equal(2, roles.Count());
    }

    [Fact]
    public void GetEmail_ThrowsIfNoEmailClaim()
    {
        // Arrange
        var accessor = CreateHttpContextAccessorWithClaims([]);
        var service = new CurrentUserService(accessor);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.GetEmail());
    }

    [Fact]
    public void GetPublicId_ThrowsIfInvalidId()
    {
        // Arrange
        var accessor = CreateHttpContextAccessorWithClaims(
        [
            new Claim(ClaimTypes.NameIdentifier, "abc")
        ]);

        var service = new CurrentUserService(accessor);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.GetPublicId());
    }
}
