using Application.Common.Interfaces;
using Application.Features.Posts.Create;
using Application.Test.Helpers;
using Domain.ApplicationUserAggregate;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.Posts.Create;

public class CreatePostHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly CreatePostHandler _handler;

    public CreatePostHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _currentUserService = TestHelpers.CreateMockCurrentUserService(1);
        _handler = new CreatePostHandler(_unitOfWork, _currentUserService);
    }

    [Fact]
    public async Task Handle_ValidInput_CreatesPostAndReturnsSuccess()
    {
        // Arrange
        var command = new CreatePostCommand("This is a test post");
        var post = new Post
        {
            Id = 1,
            Content = command.Content,
            UserId = 1,
            DatePosted = DateTime.UtcNow
        };

        _unitOfWork.PostRepository.Add(Arg.Any<Post>()).Returns(post);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        
        Assert.Equal(ResultStatus.Created, result.Status);
        
        _unitOfWork.PostRepository.Received(1).Add(Arg.Is<Post>(p =>
            p.Content == command.Content &&
            p.UserId == 1));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_EmptyContent_CreatesPostWithEmptyContent()
    {
        // Arrange
        var command = new CreatePostCommand("");
        
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _unitOfWork.PostRepository.Received(1).Add(Arg.Any<Post>());
    }
}

