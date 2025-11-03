using Infrastructure.Users;

namespace Infrastructure.Test.Users;

public class OnlineUsersStoreTests
{
    [Fact]
    public async Task AddUserConnection_FirstConnection_ReturnsTrue()
    {
        // Arrange
        OnlineUsersStore store = new();


        // Act
        bool result = await store.AddUserConnection(1, "conn1");

        // Assert
        Assert.True(result);

        var onlineUsers = await store.GetOnlineUsers();
        Assert.Single(onlineUsers);
        Assert.Equal(1, onlineUsers.First());

        var connections = await store.GetConnectionsByUserId(1);
        Assert.Single(connections);
        Assert.Contains("conn1", connections);
    }

    [Fact]
    public async Task AddUserConnection_SecondConnection_ReturnsFalse()
    {
        // Arrange
        OnlineUsersStore store = new();

        await store.AddUserConnection(1, "conn1");

        // Act
        bool result = await store.AddUserConnection(1, "conn2");

        // Assert
        Assert.False(result);

        var connections = await store.GetConnectionsByUserId(1);
        Assert.Equal(2, connections.Count);
        Assert.Contains("conn1", connections);
        Assert.Contains("conn2", connections);
    }

    [Fact]
    public async Task RemoveUserConnection_LastConnection_ReturnsTrue_AndRemovesUser()
    {
        // Arrange
        OnlineUsersStore store = new();

        await store.AddUserConnection(1, "conn1");

        // Act
        bool result = await store.RemoveUserConnection(1, "conn1");

        // Assert
        Assert.True(result);

        var onlineUsers = await store.GetOnlineUsers();
        Assert.Empty(onlineUsers);
    }

    [Fact]
    public async Task RemoveUserConnection_NotLastConnection_ReturnsFalse()
    {
        // Arrange
        OnlineUsersStore store = new();

        await store.AddUserConnection(1, "conn1");
        await store.AddUserConnection(1, "conn2");

        // Act
        bool result = await store.RemoveUserConnection(1, "conn1");

        // Assert
        Assert.False(result);

        var remaining = await store.GetConnectionsByUserId(1);
        Assert.Single(remaining);
        Assert.Contains("conn2", remaining);
    }

    [Fact]
    public async Task RemoveUserConnection_UserNotFound_ReturnsFalse()
    {
        // Arrange
        OnlineUsersStore store = new();

        // Act
        bool result = await store.RemoveUserConnection(999, "connX");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetConnectionsByUserId_UserNotFound_ReturnsEmptyList()
    {
        // Arrange
        OnlineUsersStore store = new();

        // Act
        var connections = await store.GetConnectionsByUserId(123);

        // Assert
        Assert.Empty(connections);
    }

    [Fact]
    public async Task GetOnlineUsers_ReturnsSortedList()
    {
        // Arrange
        OnlineUsersStore store = new();

        await store.AddUserConnection(5, "c5");
        await store.AddUserConnection(1, "c1");
        await store.AddUserConnection(3, "c3");

        // Act
        var onlineUsers = await store.GetOnlineUsers();

        // Assert
        Assert.Equal([1, 3, 5], onlineUsers);
    }
}
