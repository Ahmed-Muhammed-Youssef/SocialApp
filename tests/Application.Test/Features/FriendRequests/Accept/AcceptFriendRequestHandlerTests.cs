using Application.Common.Interfaces;
using Application.Features.FriendRequests.Accept;
using Application.Test.Helpers;
using Domain.FriendAggregate;
using Domain.FriendRequestAggregate;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.FriendRequests.Accept;

public class AcceptFriendRequestHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly AcceptFriendRequestHandler _handler;

    public AcceptFriendRequestHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _currentUserService = TestHelpers.CreateMockCurrentUserService(2); // Requested user
        _handler = new AcceptFriendRequestHandler(_unitOfWork, _currentUserService);
    }

    [Fact]
    public async Task Handle_ValidRequest_AcceptsAndCreatesFriend()
    {
        // Arrange
        var command = new AcceptFriendRequestCommand(1);
        var friendRequest = FriendRequest.Create(1, 2); // Requester: 1, Requested: 2
        friendRequest.GetType().GetProperty("Id")!.SetValue(friendRequest, 1);

        _unitOfWork.FriendRequestRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(friendRequest);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Created, result.Status);
        _unitOfWork.FriendRequestRepository.Received(1).Update(Arg.Any<FriendRequest>());
        _unitOfWork.FriendRepository.Received(1).Add(Arg.Any<Friend>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RequestNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = new AcceptFriendRequestCommand(999);

        _unitOfWork.FriendRequestRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((FriendRequest?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }
}

