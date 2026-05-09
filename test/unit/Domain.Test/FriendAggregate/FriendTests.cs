using Domain.FriendAggregate;
using Domain.FriendAggregate.Exceptions;

namespace Domain.Test.FriendAggregate;

public class FriendTests
{
    [Fact]
    public void CreateFromAcceptedRequest_ValidInput_CreatesFriendWithOrderedIds()
    {
        // Arrange
        var user1Id = 1;
        var user2Id = 2;
        var beforeCreation = DateTime.UtcNow;

        // Act
        var friend = Friend.CreateFromAcceptedRequest(user1Id, user2Id);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.Equal(1, friend.UserId);
        Assert.Equal(2, friend.FriendId);
        Assert.True(friend.Created >= beforeCreation);
        Assert.True(friend.Created <= afterCreation);
    }

    [Fact]
    public void CreateFromAcceptedRequest_ReversedUserIds_OrdersIdsCorrectly()
    {
        // Arrange
        var user1Id = 5;
        var user2Id = 2;

        // Act
        var friend = Friend.CreateFromAcceptedRequest(user1Id, user2Id);

        // Assert
        Assert.Equal(2, friend.UserId);
        Assert.Equal(5, friend.FriendId);
    }

    [Fact]
    public void CreateFromAcceptedRequest_SameUserIds_ThrowsInvalidFriendParametersException()
    {
        // Arrange
        var userId = 1;

        // Act & Assert
        var exception = Assert.Throws<InvalidFriendParametersException>(() =>
            Friend.CreateFromAcceptedRequest(userId, userId));

        Assert.Contains("cannot befriend yourself", exception.Message);
    }

    [Fact]
    public void CreateFromAcceptedRequest_SetsCreatedToNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var friend = Friend.CreateFromAcceptedRequest(1, 2);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(friend.Created >= beforeCreation);
        Assert.True(friend.Created <= afterCreation);
    }

    [Fact]
    public void CreateFromAcceptedRequest_MultipleFriends_EachHasUniqueCreated()
    {
        // Arrange & Act
        var friend1 = Friend.CreateFromAcceptedRequest(1, 2);
        Thread.Sleep(10);
        var friend2 = Friend.CreateFromAcceptedRequest(3, 4);
        Thread.Sleep(10);
        var friend3 = Friend.CreateFromAcceptedRequest(5, 6);

        // Assert
        Assert.True(friend2.Created > friend1.Created);
        Assert.True(friend3.Created > friend2.Created);
    }

    [Fact]
    public void CreateFromAcceptedRequest_WithNegativeIds_CreatesFriend()
    {
        // Arrange
        var user1Id = -1;
        var user2Id = -2;

        // Act
        var friend = Friend.CreateFromAcceptedRequest(user1Id, user2Id);

        // Assert
        Assert.Equal(-2, friend.UserId);
        Assert.Equal(-1, friend.FriendId);
    }

    [Fact]
    public void CreateFromAcceptedRequest_WithZeroId_CreatesFriend()
    {
        // Arrange
        var user1Id = 0;
        var user2Id = 1;

        // Act
        var friend = Friend.CreateFromAcceptedRequest(user1Id, user2Id);

        // Assert
        Assert.Equal(0, friend.UserId);
        Assert.Equal(1, friend.FriendId);
    }

    [Fact]
    public void CreateFromAcceptedRequest_LargeUserIds_CreatesFriend()
    {
        // Arrange
        var user1Id = int.MaxValue - 1;
        var user2Id = int.MaxValue;

        // Act
        var friend = Friend.CreateFromAcceptedRequest(user1Id, user2Id);

        // Assert
        Assert.Equal(int.MaxValue - 1, friend.UserId);
        Assert.Equal(int.MaxValue, friend.FriendId);
    }

    [Fact]
    public void CreateFromAcceptedRequest_ConsistentOrdering_AlwaysOrdersCorrectly()
    {
        // Arrange
        var testCases = new[]
        {
            (1, 2, 1, 2),
            (2, 1, 1, 2),
            (100, 50, 50, 100),
            (50, 100, 50, 100),
            (0, 1, 0, 1),
            (1, 0, 0, 1)
        };

        // Act & Assert
        foreach (var (input1, input2, expected1, expected2) in testCases)
        {
            var friend = Friend.CreateFromAcceptedRequest(input1, input2);
            Assert.Equal(expected1, friend.UserId);
            Assert.Equal(expected2, friend.FriendId);
        }
    }
}

