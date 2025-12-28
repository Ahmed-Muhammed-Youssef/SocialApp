using Application.Common.Interfaces;
using Application.Features.Users;
using Application.Features.Users.Get;
using Application.Test.Helpers;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.Users.Get;

public class GetUserHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetUserHandler _handler;

    public GetUserHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _handler = new GetUserHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_UserExists_ReturnsSuccess()
    {
        // Arrange
        var query = new GetUserQuery(1);
        var userDto = TestHelpers.CreateTestUserDto(1);

        _unitOfWork.ApplicationUserRepository.GetDtoByIdAsync(query.Id)
            .Returns(userDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal("John", result.Value.FirstName);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var query = new GetUserQuery(999);

        _unitOfWork.ApplicationUserRepository.GetDtoByIdAsync(query.Id)
            .Returns((UserDTO?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }
}

