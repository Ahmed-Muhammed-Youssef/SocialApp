using Application.Common.Interfaces;
using Application.Features.FriendRequests.Delete;
using Application.Test.Helpers;
using Domain.FriendRequestAggregate;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.FriendRequests.Delete;

public class DeleteFriendRequestHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly DeleteFriendRequestHandler _handler;

    public DeleteFriendRequestHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _currentUserService = TestHelpers.CreateMockCurrentUserService(1);
        _handler = new DeleteFriendRequestHandler(_unitOfWork, _currentUserService);
    }

    [Fact]
    public async Task Handle_RequestNotFound_ReturnsError()
    {
        // Arrange
        var command = new DeleteFriendRequestCommand(1);

        _unitOfWork.FriendRequestRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((FriendRequest?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _unitOfWork.FriendRequestRepository.DidNotReceive().Delete(Arg.Any<FriendRequest>());
    }

    [Fact]
    public async Task Handle_CallerIsNotRequester_ReturnsError()
    {
        // Arrange — caller (user 1) is the RequestedId, not the RequesterId; only the sender may delete.
        var command = new DeleteFriendRequestCommand(1);
        var friendRequest = FriendRequest.Create(2, 1);
        friendRequest.Id = 1;

        _unitOfWork.FriendRequestRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(friendRequest);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _unitOfWork.FriendRequestRepository.DidNotReceive().Delete(Arg.Any<FriendRequest>());
    }

    [Fact]
    public async Task Handle_RequestNotPending_ReturnsError()
    {
        // Arrange
        var command = new DeleteFriendRequestCommand(1);
        var friendRequest = FriendRequest.Create(1, 2);
        friendRequest.Id = 1;
        friendRequest.Accept(2);

        _unitOfWork.FriendRequestRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(friendRequest);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        _unitOfWork.FriendRequestRepository.DidNotReceive().Delete(Arg.Any<FriendRequest>());
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesAndReturnsNoContent()
    {
        // Arrange
        var command = new DeleteFriendRequestCommand(1);
        var friendRequest = FriendRequest.Create(1, 2);
        friendRequest.Id = 1;

        _unitOfWork.FriendRequestRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(friendRequest);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.NoContent, result.Status);
        _unitOfWork.FriendRequestRepository.Received(1).Delete(friendRequest);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}
