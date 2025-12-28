using Domain.FriendRequestAggregate;
using Domain.FriendRequestAggregate.Exceptions;

namespace Domain.Test.FriendRequestAggregate;

public class FriendRequestTests
{
    [Fact]
    public void Create_ValidInput_CreatesPendingRequest()
    {
        // Arrange
        var requesterId = 1;
        var requestedId = 2;
        var beforeCreation = DateTime.UtcNow;

        // Act
        var request = FriendRequest.Create(requesterId, requestedId);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.Equal(requesterId, request.RequesterId);
        Assert.Equal(requestedId, request.RequestedId);
        Assert.Equal(RequestStatus.Pending, request.Status);
        Assert.True(request.Date >= beforeCreation);
        Assert.True(request.Date <= afterCreation);
    }

    [Fact]
    public void Create_SameUserIds_ThrowsInvalidFriendRequestException()
    {
        // Arrange
        var userId = 1;

        // Act & Assert
        var exception = Assert.Throws<InvalidFriendRequestException>(() => 
            FriendRequest.Create(userId, userId));
        
        Assert.Contains("Cannot request self", exception.Message);
    }

    [Fact]
    public void Create_ReversedUserIds_CreatesRequest()
    {
        // Arrange
        var requesterId = 5;
        var requestedId = 2;

        // Act
        var request = FriendRequest.Create(requesterId, requestedId);

        // Assert
        Assert.Equal(requesterId, request.RequesterId);
        Assert.Equal(requestedId, request.RequestedId);
        Assert.Equal(RequestStatus.Pending, request.Status);
    }

    [Fact]
    public void Accept_ValidRequest_ChangesStatusToAccepted()
    {
        // Arrange
        var requesterId = 1;
        var requestedId = 2;
        var request = FriendRequest.Create(requesterId, requestedId);

        // Act
        request.Accept(requestedId);

        // Assert
        Assert.Equal(RequestStatus.Accepted, request.Status);
    }

    [Fact]
    public void Accept_ByRequester_ThrowsInvalidFriendRequestException()
    {
        // Arrange
        var requesterId = 1;
        var requestedId = 2;
        var request = FriendRequest.Create(requesterId, requestedId);

        // Act & Assert
        var exception = Assert.Throws<InvalidFriendRequestException>(() => 
            request.Accept(requesterId));
        
        Assert.Contains("Only pending requests can be accepted by recipient", exception.Message);
    }

    [Fact]
    public void Accept_ByNonParticipant_ThrowsInvalidFriendRequestException()
    {
        // Arrange
        var requesterId = 1;
        var requestedId = 2;
        var nonParticipantId = 99;
        var request = FriendRequest.Create(requesterId, requestedId);

        // Act & Assert
        var exception = Assert.Throws<InvalidFriendRequestException>(() => 
            request.Accept(nonParticipantId));
        
        Assert.Contains("Only pending requests can be accepted by recipient", exception.Message);
    }

    [Fact]
    public void Accept_AlreadyAcceptedRequest_ThrowsInvalidFriendRequestException()
    {
        // Arrange
        var requesterId = 1;
        var requestedId = 2;
        var request = FriendRequest.Create(requesterId, requestedId);
        request.Accept(requestedId);

        // Act & Assert
        var exception = Assert.Throws<InvalidFriendRequestException>(() => 
            request.Accept(requestedId));
        
        Assert.Contains("Only pending requests can be accepted", exception.Message);
    }

    [Fact]
    public void Create_SetsDateToUtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var request = FriendRequest.Create(1, 2);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(request.Date >= beforeCreation);
        Assert.True(request.Date <= afterCreation);
    }

    [Fact]
    public void Create_MultipleRequests_EachHasUniqueDate()
    {
        // Arrange & Act
        var request1 = FriendRequest.Create(1, 2);
        Thread.Sleep(10);
        var request2 = FriendRequest.Create(3, 4);
        Thread.Sleep(10);
        var request3 = FriendRequest.Create(5, 6);

        // Assert
        Assert.True(request2.Date > request1.Date);
        Assert.True(request3.Date > request2.Date);
    }

    [Fact]
    public void Accept_DoesNotChangeDate()
    {
        // Arrange
        var request = FriendRequest.Create(1, 2);
        var originalDate = request.Date;

        // Act
        Thread.Sleep(10);
        request.Accept(2);

        // Assert
        Assert.Equal(originalDate, request.Date);
    }

    [Fact]
    public void Create_WithNegativeIds_CreatesRequest()
    {
        // Arrange
        var requesterId = -1;
        var requestedId = -2;

        // Act
        var request = FriendRequest.Create(requesterId, requestedId);

        // Assert
        Assert.Equal(requesterId, request.RequesterId);
        Assert.Equal(requestedId, request.RequestedId);
    }

    [Fact]
    public void Create_WithZeroIds_CreatesRequest()
    {
        // Arrange
        var requesterId = 0;
        var requestedId = 1;

        // Act
        var request = FriendRequest.Create(requesterId, requestedId);

        // Assert
        Assert.Equal(requesterId, request.RequesterId);
        Assert.Equal(requestedId, request.RequestedId);
    }
}

