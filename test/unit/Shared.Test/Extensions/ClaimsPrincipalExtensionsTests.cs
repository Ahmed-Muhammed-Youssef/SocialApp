using System.Security.Claims;
using Shared.Extensions;

namespace Shared.Test.Extensions;

public class ClaimsPrincipalExtensionsTests
{
    private static ClaimsPrincipal CreatePrincipal(params Claim[] claims)
    {
        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ClaimsPrincipal(identity);
    }

    [Fact]
    public void GetPublicId_ValidNameIdentifier_ReturnsId()
    {
        // Arrange
        var principal = CreatePrincipal(new Claim(ClaimTypes.NameIdentifier, "42"));

        // Act
        var id = principal.GetPublicId();

        // Assert
        Assert.Equal(42, id);
    }

    [Fact]
    public void GetPublicId_MissingClaim_Throws()
    {
        // Arrange
        var principal = CreatePrincipal();

        // Act & Assert
        Assert.Throws<Exception>(() => principal.GetPublicId());
    }

    [Fact]
    public void GetPublicId_NonNumericClaim_Throws()
    {
        // Arrange
        var principal = CreatePrincipal(new Claim(ClaimTypes.NameIdentifier, "not-a-number"));

        // Act & Assert
        Assert.Throws<Exception>(() => principal.GetPublicId());
    }

    [Fact]
    public void GetEmail_ClaimPresent_ReturnsValue()
    {
        // Arrange
        var principal = CreatePrincipal(new Claim(ClaimTypes.Email, "user@test.com"));

        // Act
        var email = principal.GetEmail();

        // Assert
        Assert.Equal("user@test.com", email);
    }

    [Fact]
    public void GetEmail_MissingClaim_ReturnsEmptyString()
    {
        // Arrange
        var principal = CreatePrincipal();

        // Act
        var email = principal.GetEmail();

        // Assert
        Assert.Equal(string.Empty, email);
    }

    [Fact]
    public void GetRoles_MultipleRoleClaims_ReturnsAll()
    {
        // Arrange
        var principal = CreatePrincipal(
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.Role, "Admin"));

        // Act
        var roles = principal.GetRoles().ToList();

        // Assert
        Assert.Equal(2, roles.Count);
        Assert.Contains("User", roles);
        Assert.Contains("Admin", roles);
    }

    [Fact]
    public void GetRoles_NoRoleClaims_ReturnsEmpty()
    {
        // Arrange
        var principal = CreatePrincipal();

        // Act
        var roles = principal.GetRoles();

        // Assert
        Assert.Empty(roles);
    }
}
