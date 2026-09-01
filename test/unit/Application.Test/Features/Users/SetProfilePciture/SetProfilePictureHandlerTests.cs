using Application.Common.Interfaces;
using Application.Features.Users.SetProfilePciture;
using Application.Test.Helpers;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.Users.SetProfilePciture;

public class SetProfilePictureHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly SetProfilePictureHandler _handler;

    public SetProfilePictureHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _currentUserService = TestHelpers.CreateMockCurrentUserService(1);
        _handler = new SetProfilePictureHandler(_unitOfWork, _currentUserService);
    }

    [Fact]
    public async Task Handle_PictureNotOwnedOrMissing_ReturnsError()
    {
        // Arrange
        var command = new SetProfilePictureCommand(99);

        _unitOfWork.ApplicationUserRepository.SetProfilePictureIfOwnedAsync(1, 99, Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Error, result.Status);
    }

    [Fact]
    public async Task Handle_PictureOwnedByCaller_ReturnsSuccess()
    {
        // Arrange
        var command = new SetProfilePictureCommand(5);

        // The user id must come from ICurrentUserService (the caller), and only the picture id
        // from the command — a handler that took the user id from the command would let a caller
        // set someone else's profile picture.
        _unitOfWork.ApplicationUserRepository.SetProfilePictureIfOwnedAsync(1, 5, Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _unitOfWork.ApplicationUserRepository.Received(1)
            .SetProfilePictureIfOwnedAsync(1, 5, Arg.Any<CancellationToken>());
    }
}
