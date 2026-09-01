using Application.Common.Interfaces;
using Application.Features.UserRoles.RemoveRoleFromUser;
using Application.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.UserRoles.RemoveRoleFromUser;

public class RemoveRoleFromUserHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RemoveRoleFromUserHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();

        var roleStore = Substitute.For<IRoleStore<IdentityRole>>();
        _roleManager = Substitute.For<RoleManager<IdentityRole>>(roleStore, null!, null!, null!, null!);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = new RemoveRoleFromUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((Domain.ApplicationUserAggregate.ApplicationUser?)null);

        using var userManager = UserManagerTestHelper.CreateUserManagerWithUsers();
        var handler = new RemoveRoleFromUserHandler(_roleManager, userManager, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_IdentityUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser(1, "identity-1");
        var command = new RemoveRoleFromUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        // No identity users seeded, so FindByIdAsync("identity-1") returns null.
        using var userManager = UserManagerTestHelper.CreateUserManagerWithUsers();
        var handler = new RemoveRoleFromUserHandler(_roleManager, userManager, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_RoleNotFound_ReturnsNotFound()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser(1, "identity-1");
        var identityUser = TestHelpers.CreateTestIdentityUser("identity-1");
        var command = new RemoveRoleFromUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        _roleManager.FindByIdAsync(Arg.Any<string>()).Returns((IdentityRole?)null);

        using var userManager = UserManagerTestHelper.CreateUserManagerWithUsers([identityUser]);
        var handler = new RemoveRoleFromUserHandler(_roleManager, userManager, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_ValidRequest_RemovesRequestedRoleAndReturnsNoContent()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser(1, "identity-1");
        var identityUser = TestHelpers.CreateTestIdentityUser("identity-1");
        var role = new IdentityRole { Id = "role-1", Name = "Moderator", NormalizedName = "MODERATOR" };
        var command = new RemoveRoleFromUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        // Must look the role up by command.RoleId, not by the user's identity id.
        _roleManager.FindByIdAsync("role-1").Returns(role);
        _roleManager.FindByIdAsync("identity-1").Returns((IdentityRole?)null);

        using var userManager = UserManagerTestHelper.CreateUserManagerWithUsers([identityUser], [role]);

        // Seed the user into the role first so there is something to remove.
        await userManager.AddToRoleAsync(identityUser, role.Name);

        var handler = new RemoveRoleFromUserHandler(_roleManager, userManager, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.NoContent, result.Status);
        Assert.False(await userManager.IsInRoleAsync(identityUser, role.Name));
    }

    [Fact]
    public async Task Handle_UserNotInRole_ReturnsError()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser(1, "identity-1");
        var identityUser = TestHelpers.CreateTestIdentityUser("identity-1");
        var role = new IdentityRole { Id = "role-1", Name = "Moderator" };
        var command = new RemoveRoleFromUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        _roleManager.FindByIdAsync("role-1").Returns(role);

        // UserManagerTestHelper's real UserManager needs a non-null logger to reach failure paths
        // like RemoveFromRoleAsync's "not in role" branch, so this test mocks UserManager directly
        // instead of using the real store-backed one.
        var userStore = Substitute.For<IUserStore<IdentityUser>>();
        var userManager = Substitute.For<UserManager<IdentityUser>>(userStore, null!, null!, null!, null!, null!, null!, null!, null!);

        userManager.FindByIdAsync("identity-1").Returns(identityUser);
        userManager.RemoveFromRoleAsync(identityUser, "Moderator")
            .Returns(IdentityResult.Failed(new IdentityError { Description = "User is not in role." }));

        var handler = new RemoveRoleFromUserHandler(_roleManager, userManager, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.NotEmpty(result.Errors);
    }
}
