using Application.Common.Interfaces;
using Application.Features.Auth;
using Application.Features.Auth.Login;
using Application.Features.Users;
using Application.Features.Users.Specifications;
using Application.Test.Helpers;
using Domain.ApplicationUserAggregate;
using Domain.AuthUserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shared.Constants;
using Shared.Results;
using Shared.Specification;

namespace Application.Test.Features.Auth.Login;

public class LoginHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    private readonly IApplicationDatabaseContext _identityDbContext;

    public LoginHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _tokenProvider = Substitute.For<ITokenProvider>();
        _identityDbContext = Substitute.For<IApplicationDatabaseContext>();
    }

    private SignInManager<IdentityUser> CreateSignInManager(UserManager<IdentityUser> userManager)
    {
        return Substitute.For<SignInManager<IdentityUser>>(
            userManager,
            Substitute.For<IHttpContextAccessor>(),
            Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>(),
            null!, null!, null!, null!);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "Password123!");
        var identityUser = TestHelpers.CreateTestIdentityUser("identity-1", "test@test.com");
        var userDto = TestHelpers.CreateTestUserDto(1);

        var userManager = UserManagerTestHelper.CreateUserManagerWithUsers([identityUser], [new IdentityRole { Name = RolesNameValues.User, NormalizedName = RolesNameValues.User.ToUpper() }]);
        var signInManager = CreateSignInManager(userManager);

        signInManager.CheckPasswordSignInAsync(identityUser, command.Password, false)
            .Returns(SignInResult.Success);

        _unitOfWork.ApplicationUserRepository.GetDtoByIdentityAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(userDto);

        _tokenProvider.CreateAccessToken(Arg.Any<TokenRequest>()).Returns("access-token");

        var refreshToken = new RefreshToken
        {
            UserId = identityUser.Id,
            Token = "refresh-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };
        _tokenProvider.CreateRefreshToken(identityUser.Id).Returns(refreshToken);
        _identityDbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        var handler = new LoginHandler(_unitOfWork, userManager, signInManager, _tokenProvider, _identityDbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("access-token", result.Value.Token);
        Assert.Equal("refresh-token", result.Value.RefreshToken);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var command = new LoginCommand("nonexistent@test.com", "Password123!");

        var userManager = UserManagerTestHelper.CreateUserManagerWithUsers([]); // Empty list
        var signInManager = CreateSignInManager(userManager);
        var handler = new LoginHandler(_unitOfWork, userManager, signInManager, _tokenProvider, _identityDbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Unauthorized, result.Status);
    }

    [Fact]
    public async Task Handle_InvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "WrongPassword");
        var identityUser = TestHelpers.CreateTestIdentityUser("identity-1", "test@test.com");

        var userManager = UserManagerTestHelper.CreateUserManagerWithUsers([identityUser]);
        var signInManager = CreateSignInManager(userManager);

        signInManager.CheckPasswordSignInAsync(identityUser, command.Password, false)
            .Returns(SignInResult.Failed);

        var handler = new LoginHandler(_unitOfWork, userManager, signInManager, _tokenProvider, _identityDbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Unauthorized, result.Status);
    }

    [Fact]
    public async Task Handle_ApplicationUserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "Password123!");
        var identityUser = TestHelpers.CreateTestIdentityUser("identity-1", "test@test.com");

        var userManager = UserManagerTestHelper.CreateUserManagerWithUsers([identityUser]);
        var signInManager = CreateSignInManager(userManager);

        signInManager.CheckPasswordSignInAsync(identityUser, command.Password, false)
            .Returns(SignInResult.Success);

        _unitOfWork.ApplicationUserRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<ApplicationUser, UserDTO>>(), Arg.Any<CancellationToken>())
            .Returns((UserDTO?)null);

        var handler = new LoginHandler(_unitOfWork, userManager, signInManager, _tokenProvider, _identityDbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Unauthorized, result.Status);
    }
}

