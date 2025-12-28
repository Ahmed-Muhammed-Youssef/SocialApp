using Domain.DirectChatAggregate.Exceptions;
using Domain.FriendAggregate.Exceptions;
using Domain.FriendRequestAggregate.Exceptions;
using Shared;

namespace Domain.Test;

public class DomainExceptionsTests
{
    [Fact]
    public void InvalidSenderException_InheritsFromDomainException()
    {
        // Arrange & Act
        var exception = new InvalidSenderException("Test reason");

        // Assert
        Assert.IsType<DomainException>(exception, exactMatch: false);
        Assert.IsType<Exception>(exception, exactMatch: false);
    }

    [Fact]
    public void InvalidSenderException_ContainsReasonInMessage()
    {
        // Arrange
        var reason = "Sender is not a participant";

        // Act
        var exception = new InvalidSenderException(reason);

        // Assert
        Assert.Contains(reason, exception.Message);
        Assert.Contains("Sender is invalid", exception.Message);
    }

    [Fact]
    public void InvalidFriendRequestException_InheritsFromDomainException()
    {
        // Arrange & Act
        var exception = new InvalidFriendRequestException("Test reason");

        // Assert
        Assert.IsType<DomainException>(exception, exactMatch: false);
        Assert.IsType<Exception>(exception, exactMatch: false);
    }

    [Fact]
    public void InvalidFriendRequestException_ContainsReasonInMessage()
    {
        // Arrange
        var reason = "Cannot request self";

        // Act
        var exception = new InvalidFriendRequestException(reason);

        // Assert
        Assert.Contains(reason, exception.Message);
        Assert.Contains("Friend request is invalid", exception.Message);
    }

    [Fact]
    public void InvalidFriendParametersException_InheritsFromDomainException()
    {
        // Arrange & Act
        var exception = new InvalidFriendParametersException("Test reason");

        // Assert
        Assert.IsType<DomainException>(exception, exactMatch: false);
        Assert.IsType<Exception>(exception, exactMatch: false);
    }

    [Fact]
    public void InvalidFriendParametersException_ContainsReasonInMessage()
    {
        // Arrange
        var reason = "Cannot befriend yourself";

        // Act
        var exception = new InvalidFriendParametersException(reason);

        // Assert
        Assert.Contains(reason, exception.Message);
        Assert.Contains("Parameters are invalid", exception.Message);
    }

    [Fact]
    public void DomainException_CanBeCaughtAsBaseException()
    {
        // Arrange
        Exception? caughtException = null;

        // Act
        try
        {
            throw new InvalidSenderException("Test");
        }
        catch (DomainException ex)
        {
            caughtException = ex;
        }

        // Assert
        Assert.NotNull(caughtException);
        Assert.IsType<InvalidSenderException>(caughtException);
    }

    [Fact]
    public void DomainException_MessageIsPreserved()
    {
        // Arrange
        var expectedMessage = "Sender is invalid: Test reason";

        // Act
        var exception = new InvalidSenderException("Test reason");

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }
}

