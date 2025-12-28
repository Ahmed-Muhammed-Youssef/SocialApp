using Application.Common.Interfaces;
using Application.Features.FriendRequests.Create;
using Application.Test.Helpers;
using Domain.FriendRequestAggregate;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.FriendRequests.Create;

public class CreateFriendRequestHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly CreateFriendRequestHandler _handler;

    public CreateFriendRequestHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _currentUserService = TestHelpers.CreateMockCurrentUserService(1);
        _handler = new CreateFriendRequestHandler(_unitOfWork, _currentUserService);
    }

    [Fact]
    public async Task Handle_ValidInput_CreatesFriendRequestAndReturnsSuccess()
    {
        // Arrange
        var command = new CreateFriendRequestCommand(2);
        var friendRequest = FriendRequest.Create(1, 2);

        _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(2, 1)
            .Returns((FriendRequest?)null);
        
        _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(1, 2)
            .Returns((FriendRequest?)null);
        
        _unitOfWork.FriendRequestRepository.IsFriend(1, 2)
            .Returns(false);
        
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Created, result.Status);
        _unitOfWork.FriendRequestRepository.Received(1).Add(Arg.Any<FriendRequest>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_PendingRequestFromTarget_ReturnsError()
    {
        // Arrange
        var command = new CreateFriendRequestCommand(2);
        var existingRequest = FriendRequest.Create(2, 1);
        existingRequest.GetType().GetProperty("Status")!.SetValue(existingRequest, RequestStatus.Pending);

        _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(2, 1)
            .Returns(existingRequest);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("already have a pending", result.Errors.First());
    }

    [Fact]
    public async Task Handle_AlreadySentRequest_ReturnsError()
    {
        // Arrange
        var command = new CreateFriendRequestCommand(2);
        var existingRequest = FriendRequest.Create(1, 2);

        _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(2, 1)
            .Returns((FriendRequest?)null);
        
        _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(1, 2)
            .Returns(existingRequest);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("already sent", result.Errors.First());
    }

    [Fact]
    public async Task Handle_AlreadyFriends_ReturnsError()
    {
        // Arrange
        var command = new CreateFriendRequestCommand(2);

        _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(2, 1)
            .Returns((FriendRequest?)null);
        
        _unitOfWork.FriendRequestRepository.GetFriendRequestAsync(1, 2)
            .Returns((FriendRequest?)null);
        
        _unitOfWork.FriendRequestRepository.IsFriend(1, 2)
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("already are friends", result.Errors.First());
    }
}

