using Application.Features.Roles.Create;
using Application.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.Roles.Create;

public class CreateRoleHandlerTests
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly CreateRoleHandler _handler;

    public CreateRoleHandlerTests()
    {
        var roleStore = Substitute.For<IRoleStore<IdentityRole>>();
        _roleManager = Substitute.For<RoleManager<IdentityRole>>(
            roleStore, null!, null!, null!, null!);
        _handler = new CreateRoleHandler(_roleManager);
    }

    [Fact]
    public async Task Handle_ValidRoleName_CreatesRoleAndReturnsSuccess()
    {
        // Arrange
        var command = new CreateRoleCommand("Moderator");
        var role = new IdentityRole { Id = "role-1", Name = command.Name };

        _roleManager.CreateAsync(Arg.Any<IdentityRole>()).Returns(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Created, result.Status);
        await _roleManager.Received(1).CreateAsync(Arg.Any<IdentityRole>());
    }

    [Fact]
    public async Task Handle_CreationFails_ReturnsError()
    {
        // Arrange
        var command = new CreateRoleCommand("Moderator");

        _roleManager.RoleExistsAsync(command.Name).Returns(false);
        _roleManager.CreateAsync(Arg.Any<IdentityRole>())
            .Returns(IdentityResult.Failed(new IdentityError { Description = "Creation failed" }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
    }
}

