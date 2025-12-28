using Application.Common.Interfaces;
using Application.Features.Posts.GetById;
using Application.Test.Helpers;
using Domain.ApplicationUserAggregate;
using NSubstitute;
using Shared.Results;

namespace Application.Test.Features.Posts.GetById;

public class GetPostByIdHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetPostByIdHandler _handler;

    public GetPostByIdHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _handler = new GetPostByIdHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_PostExists_ReturnsSuccess()
    {
        // Arrange
        var query = new GetPostByIdQuery(1);
        var post = new Post
        {
            Id = 1,
            Content = "Test post",
            UserId = 1,
            DatePosted = DateTime.UtcNow
        };

        _unitOfWork.PostRepository.GetByIdAsync(query.PostId, Arg.Any<CancellationToken>())
            .Returns(post);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(1UL, result.Value.Id);
        Assert.Equal("Test post", result.Value.Content);
    }

    [Fact]
    public async Task Handle_PostNotFound_ReturnsNotFound()
    {
        // Arrange
        var query = new GetPostByIdQuery(999);

        _unitOfWork.PostRepository.GetByIdAsync(query.PostId, Arg.Any<CancellationToken>())
            .Returns((Post?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Contains("not found", result.Errors.First());
    }
}

