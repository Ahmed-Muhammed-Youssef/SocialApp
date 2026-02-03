using Application.Common.Interfaces;
using Application.Features.Users.Posts;
using Application.Features.Users.Posts.List;
using Application.Test.Helpers;
using NSubstitute;
using Shared.Pagination;
using Shared.Results;

namespace Application.Test.Features.Posts.List;

public class GetPostsHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly GetPostsHandler _handler;

    public GetPostsHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _currentUserService = TestHelpers.CreateMockCurrentUserService(1);
        _handler = new GetPostsHandler(_unitOfWork, _currentUserService);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsPagedList()
    {
        // Arrange
        var paginationParams = new PaginationParams { PageNumber = 1, ItemsPerPage = 10 };
        var query = new GetPostsQuery(paginationParams);
        var posts = new PagedList<PostDTO>([], 0, 1, 10);

        _unitOfWork.ApplicationUserRepository.GetNewsfeed(1, paginationParams, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(posts));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}

