using Domain.DirectChatAggregate;

namespace Domain.Test.DirectChatAggregate;

public class MessageTests
{
    [Fact]
    public void Constructor_ValidInput_CreatesMessageWithCorrectProperties()
    {
        // Arrange
        var chatId = 1;
        var senderId = 2;
        var content = "Test message content";
        var beforeCreation = DateTime.UtcNow;

        // Act
        var message = new Message(chatId, senderId, content);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.Equal(chatId, message.ChatId);
        Assert.Equal(senderId, message.SenderId);
        Assert.Equal(content, message.Content);
        Assert.True(message.SentDate >= beforeCreation);
        Assert.True(message.SentDate <= afterCreation);
    }

    [Fact]
    public void Constructor_SetsSentDateToUtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var message = new Message(1, 1, "Test");
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(message.SentDate >= beforeCreation);
        Assert.True(message.SentDate <= afterCreation);
        Assert.Equal(DateTimeKind.Utc, message.SentDate.Kind);
    }

    [Fact]
    public void Constructor_WithLongContent_CreatesMessage()
    {
        // Arrange
        var longContent = new string('A', 5000);

        // Act
        var message = new Message(1, 1, longContent);

        // Assert
        Assert.Equal(longContent, message.Content);
        Assert.Equal(5000, message.Content.Length);
    }

    [Fact]
    public void Constructor_WithSpecialCharacters_CreatesMessage()
    {
        // Arrange
        var specialContent = "Hello! @#$%^&*()_+-=[]{}|;':\",./<>?`~";

        // Act
        var message = new Message(1, 1, specialContent);

        // Assert
        Assert.Equal(specialContent, message.Content);
    }

    [Fact]
    public void Constructor_WithUnicodeCharacters_CreatesMessage()
    {
        // Arrange
        var unicodeContent = "Hello 世界 🌍 مرحبا";

        // Act
        var message = new Message(1, 1, unicodeContent);

        // Assert
        Assert.Equal(unicodeContent, message.Content);
    }

    [Fact]
    public void Content_IsReadOnly()
    {
        // Arrange
        var property = typeof(Message).GetProperty(nameof(Message.Content));

        // Act & Assert
        Assert.NotNull(property);
        Assert.False(property.SetMethod?.IsPublic ?? false);
    }

    [Fact]
    public void SentDate_IsReadOnly()
    {
        // Arrange
        var property = typeof(Message).GetProperty(nameof(Message.SentDate));

        // Act & Assert
        Assert.NotNull(property);
        Assert.False(property.SetMethod?.IsPublic ?? false);
    }

    [Fact]
    public void SenderId_IsReadOnly()
    {
        // Arrange
        var property = typeof(Message).GetProperty(nameof(Message.SenderId));

        // Act & Assert
        Assert.NotNull(property);
        Assert.False(property.SetMethod?.IsPublic ?? false);
    }
}

