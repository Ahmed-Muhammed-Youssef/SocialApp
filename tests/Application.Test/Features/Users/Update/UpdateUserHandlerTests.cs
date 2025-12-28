using Application.Common.Interfaces;
using Application.Features.Users;
using Application.Features.Users.Update;
using Application.Test.Helpers;
using Domain.ApplicationUserAggregate;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.Users.Update;

public class UpdateUserHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly UpdateUserHandler _handler;

    public UpdateUserHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _currentUserService = TestHelpers.CreateMockCurrentUserService(1);
        _handler = new UpdateUserHandler(_unitOfWork, _currentUserService);
    }

    [Fact]
    public async Task Handle_ValidInput_UpdatesUserAndReturnsSuccess()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            FirstName = "Jane",
            LastName = "Smith",
            Bio = "Updated bio",
            CityId = 2
        };
        var user = TestHelpers.CreateTestUser(1);

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);
        
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Jane", result.Value.FirstName);
        Assert.Equal("Smith", result.Value.LastName);
        Assert.Equal("Updated bio", result.Value.Bio);
        _unitOfWork.ApplicationUserRepository.Received(1).Update(Arg.Any<ApplicationUser>());
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            FirstName = "Jane",
            LastName = "Smith",
            Bio = "Updated bio",
            CityId = 2
        };

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((ApplicationUser?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
        _unitOfWork.ApplicationUserRepository.DidNotReceive().Update(Arg.Any<ApplicationUser>());
    }
}

