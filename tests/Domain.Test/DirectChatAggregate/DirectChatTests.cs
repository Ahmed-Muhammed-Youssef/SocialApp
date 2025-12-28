using Domain.DirectChatAggregate;
using Domain.DirectChatAggregate.Exceptions;

namespace Domain.Test.DirectChatAggregate;

public class DirectChatTests
{
    [Fact]
    public void Constructor_ValidUserIds_CreatesChatWithOrderedIds()
    {
        // Arrange
        var user1Id = 1;
        var user2Id = 2;

        // Act
        var chat = new DirectChat(user1Id, user2Id);

        // Assert
        Assert.Equal(1, chat.User1Id);
        Assert.Equal(2, chat.User2Id);
        Assert.Empty(chat.Messages);
    }

    [Fact]
    public void Constructor_ReversedUserIds_OrdersIdsCorrectly()
    {
        // Arrange
        var user1Id = 5;
        var user2Id = 2;

        // Act
        var chat = new DirectChat(user1Id, user2Id);

        // Assert
        Assert.Equal(2, chat.User1Id);
        Assert.Equal(5, chat.User2Id);
    }

    [Fact]
    public void Constructor_SameUserIds_OrdersIdsCorrectly()
    {
        // Arrange
        var userId = 3;

        // Act
        var chat = new DirectChat(userId, userId);

        // Assert
        Assert.Equal(3, chat.User1Id);
        Assert.Equal(3, chat.User2Id);
    }

    [Fact]
    public void AddMessage_ValidMessage_AddsMessageToChat()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 1;
        var content = "Hello, this is a test message";

        // Act
        var message = chat.AddMessage(senderId, content);

        // Assert
        Assert.Single(chat.Messages);
        Assert.Equal(message, chat.Messages.First());
        Assert.Equal(content, message.Content);
        Assert.Equal(senderId, message.SenderId);
        Assert.Equal(chat.Id, message.ChatId);
    }

    [Fact]
    public void AddMessage_MultipleMessages_AddsAllMessages()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 1;

        // Act
        var message1 = chat.AddMessage(senderId, "First message");
        var message2 = chat.AddMessage(senderId, "Second message");
        var message3 = chat.AddMessage(2, "Third message from user 2");

        // Assert
        Assert.Equal(3, chat.Messages.Count);
        Assert.Contains(message1, chat.Messages);
        Assert.Contains(message2, chat.Messages);
        Assert.Contains(message3, chat.Messages);
    }

    [Fact]
    public void AddMessage_FromUser1_AddsMessageSuccessfully()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 1;
        var content = "Message from user 1";

        // Act
        var message = chat.AddMessage(senderId, content);

        // Assert
        Assert.NotNull(message);
        Assert.Equal(senderId, message.SenderId);
    }

    [Fact]
    public void AddMessage_FromUser2_AddsMessageSuccessfully()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 2;
        var content = "Message from user 2";

        // Act
        var message = chat.AddMessage(senderId, content);

        // Assert
        Assert.NotNull(message);
        Assert.Equal(senderId, message.SenderId);
    }

    [Fact]
    public void AddMessage_NonParticipantSender_ThrowsInvalidSenderException()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var nonParticipantId = 99;
        var content = "Message from non-participant";

        // Act & Assert
        var exception = Assert.Throws<InvalidSenderException>(() => 
            chat.AddMessage(nonParticipantId, content));
        
        Assert.Contains("not a participant", exception.Message);
    }

    [Fact]
    public void AddMessage_NullContent_ThrowsInvalidMessageException()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 1;

        // Act & Assert
        var exception = Assert.Throws<InvalidMessageException>(() => 
            chat.AddMessage(senderId, null!));
        
        Assert.Contains("cannot be null or empty", exception.Message);
    }

    [Fact]
    public void AddMessage_EmptyContent_ThrowsInvalidMessageException()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 1;

        // Act & Assert
        var exception = Assert.Throws<InvalidMessageException>(() => 
            chat.AddMessage(senderId, ""));
        
        Assert.Contains("cannot be null or empty", exception.Message);
    }

    [Fact]
    public void AddMessage_WhitespaceContent_ThrowsInvalidMessageException()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 1;

        // Act & Assert
        var exception = Assert.Throws<InvalidMessageException>(() => 
            chat.AddMessage(senderId, "   "));
        
        Assert.Contains("cannot be null or empty", exception.Message);
    }

    [Fact]
    public void AddMessage_LongContent_AddsMessageSuccessfully()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 1;
        var longContent = new string('A', 10000); // 10,000 character message

        // Act
        var message = chat.AddMessage(senderId, longContent);

        // Assert
        Assert.NotNull(message);
        Assert.Equal(longContent, message.Content);
    }

    [Fact]
    public void AddMessage_SetsSentDate()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        var senderId = 1;
        var beforeMessage = DateTime.UtcNow;

        // Act
        var message = chat.AddMessage(senderId, "Test message");
        var afterMessage = DateTime.UtcNow;

        // Assert
        Assert.True(message.SentDate >= beforeMessage);
        Assert.True(message.SentDate <= afterMessage);
    }

    [Fact]
    public void OrderIds_User1LessThanUser2_ReturnsOrdered()
    {
        // Arrange
        var user1Id = 1;
        var user2Id = 2;

        // Act
        var (orderedUser1, orderedUser2) = DirectChat.OrderIds(user1Id, user2Id);

        // Assert
        Assert.Equal(1, orderedUser1);
        Assert.Equal(2, orderedUser2);
    }

    [Fact]
    public void OrderIds_User1GreaterThanUser2_ReturnsReversed()
    {
        // Arrange
        var user1Id = 5;
        var user2Id = 2;

        // Act
        var (orderedUser1, orderedUser2) = DirectChat.OrderIds(user1Id, user2Id);

        // Assert
        Assert.Equal(2, orderedUser1);
        Assert.Equal(5, orderedUser2);
    }

    [Fact]
    public void OrderIds_SameUserIds_ReturnsSame()
    {
        // Arrange
        var userId = 3;

        // Act
        var (orderedUser1, orderedUser2) = DirectChat.OrderIds(userId, userId);

        // Assert
        Assert.Equal(3, orderedUser1);
        Assert.Equal(3, orderedUser2);
    }

    [Fact]
    public void OrderIds_NegativeUserIds_OrdersCorrectly()
    {
        // Arrange
        var user1Id = -5;
        var user2Id = -2;

        // Act
        var (orderedUser1, orderedUser2) = DirectChat.OrderIds(user1Id, user2Id);

        // Assert
        Assert.Equal(-5, orderedUser1);
        Assert.Equal(-2, orderedUser2);
    }

    [Fact]
    public void Messages_IsReadOnlyCollection()
    {
        // Arrange
        var chat = new DirectChat(1, 2);
        chat.AddMessage(1, "Test");

        // Act & Assert
        Assert.IsType<IReadOnlyCollection<Message>>(chat.Messages, exactMatch: false);
        
        // Verify it's read-only - should not have Add method
        var messagesType = chat.Messages.GetType();
        var addMethod = messagesType.GetMethod("Add");
        Assert.Null(addMethod);
    }
}

