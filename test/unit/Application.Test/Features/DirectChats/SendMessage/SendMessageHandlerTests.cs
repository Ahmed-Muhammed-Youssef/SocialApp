using Application.Common.Interfaces;
using Application.Features.DirectChats;
using Application.Features.DirectChats.SendMessage;
using Application.Features.DirectChats.Stores;
using Application.Test.Helpers;
using Domain.ApplicationUserAggregate;
using Domain.DirectChatAggregate;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.DirectChats.SendMessage;

public class SendMessageHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDirectChatGroupsStore _groupsStore;
    private readonly SendMessageHandler _handler;

    public SendMessageHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _groupsStore = Substitute.For<IDirectChatGroupsStore>();
        _handler = new SendMessageHandler(_unitOfWork, _groupsStore);
    }

    [Fact]
    public async Task Handle_ValidInput_SendsMessageAndReturnsSuccess()
    {
        // Arrange
        var command = new SendMessageCommand(1, 2, "Hello!");
        var sender = TestHelpers.CreateTestUser(1);
        var recipient = TestHelpers.CreateTestUser(2);
        var chat = new DirectChat(1, 2);
        var group = new Group("group-1-2", []);

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(sender);

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(recipient);

        _unitOfWork.DirectChatRepository.GetOrAddAsync(1, 2, Arg.Any<CancellationToken>())
            .Returns(chat);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        _groupsStore.GetOrAddGroup(1, 2).Returns(group);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Hello!", result.Value.MessageDTO.Content);
    }

    [Fact]
    public async Task Handle_SenderNotFound_ReturnsError()
    {
        // Arrange
        var command = new SendMessageCommand(1, 2, "Hello!");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((ApplicationUser?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("User not found", result.Errors);
    }

    [Fact]
    public async Task Handle_RecipientNotFound_ReturnsError()
    {
        // Arrange
        var command = new SendMessageCommand(1, 2, "Hello!");
        var sender = TestHelpers.CreateTestUser(1);

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(sender);

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns((ApplicationUser?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("User not found", result.Errors);
    }

    [Fact]
    public async Task Handle_SendingToSelf_ReturnsError()
    {
        // Arrange
        var command = new SendMessageCommand(1, 1, "Hello!");
        var user = TestHelpers.CreateTestUser(1);

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Contains("can't send messages to yourself", result.Errors.First());
    }
}

