using Application.Common.Interfaces;
using Application.Features.UserRoles.AssignRoleToUser;
using Application.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.UserRoles.AssignRoleToUser;

public class AssignRoleToUserHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AssignRoleToUserHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();

        var roleStore = Substitute.For<IRoleStore<IdentityRole>>();
        _roleManager = Substitute.For<RoleManager<IdentityRole>>(roleStore, null!, null!, null!, null!);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = new AssignRoleToUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((Domain.ApplicationUserAggregate.ApplicationUser?)null);

        using var userManager = UserManagerTestHelper.CreateUserManagerWithUsers();
        var handler = new AssignRoleToUserHandler(_roleManager, userManager, _unitOfWork);

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
        var command = new AssignRoleToUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        // No identity users seeded, so FindByIdAsync("identity-1") returns null.
        using var userManager = UserManagerTestHelper.CreateUserManagerWithUsers();
        var handler = new AssignRoleToUserHandler(_roleManager, userManager, _unitOfWork);

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
        var command = new AssignRoleToUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        // Stub any id — this test asserts "role missing" behavior, not which id gets queried.
        _roleManager.FindByIdAsync(Arg.Any<string>()).Returns((IdentityRole?)null);

        using var userManager = UserManagerTestHelper.CreateUserManagerWithUsers([identityUser]);
        var handler = new AssignRoleToUserHandler(_roleManager, userManager, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_ValidRequest_AssignsRequestedRoleAndReturnsSuccess()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser(1, "identity-1");
        var identityUser = TestHelpers.CreateTestIdentityUser("identity-1");
        var role = new IdentityRole { Id = "role-1", Name = "Moderator", NormalizedName = "MODERATOR" };
        var command = new AssignRoleToUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        // The handler must look the role up by command.RoleId, not by the user's identity id.
        _roleManager.FindByIdAsync("role-1").Returns(role);
        _roleManager.FindByIdAsync("identity-1").Returns((IdentityRole?)null);

        // UserManager.AddToRoleAsync resolves the role against its OWN backing store (by
        // NormalizedName), independently of what the mocked RoleManager returns above.
        using var userManager = UserManagerTestHelper.CreateUserManagerWithUsers([identityUser], [role]);
        var handler = new AssignRoleToUserHandler(_roleManager, userManager, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        // Assert the side effect and the lookup argument, not just the status: this is the
        // regression guard for the RoleId/IdentityId mix-up, so it must be explicit.
        Assert.True(await userManager.IsInRoleAsync(identityUser, role.Name));
        await _roleManager.Received(1).FindByIdAsync("role-1");
        await _roleManager.DidNotReceive().FindByIdAsync("identity-1");
    }

    [Fact]
    public async Task Handle_AddToRoleFails_ReturnsError()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser(1, "identity-1");
        var identityUser = TestHelpers.CreateTestIdentityUser("identity-1");
        var role = new IdentityRole { Id = "role-1", Name = "Moderator" };
        var command = new AssignRoleToUserCommand(1, "role-1");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        _roleManager.FindByIdAsync("role-1").Returns(role);

        // UserManagerTestHelper's real UserManager needs a non-null logger to reach failure paths
        // like AddToRoleAsync's "already in role" branch, so this test mocks UserManager directly
        // instead — mirroring how _roleManager is mocked above.
        var userStore = Substitute.For<IUserStore<IdentityUser>>();
        var userManager = Substitute.For<UserManager<IdentityUser>>(userStore, null!, null!, null!, null!, null!, null!, null!, null!);

        userManager.FindByIdAsync("identity-1").Returns(identityUser);
        userManager.AddToRoleAsync(identityUser, "Moderator")
            .Returns(IdentityResult.Failed(new IdentityError { Description = "User already in role." }));

        var handler = new AssignRoleToUserHandler(_roleManager, userManager, _unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.NotEmpty(result.Errors);
    }
}
