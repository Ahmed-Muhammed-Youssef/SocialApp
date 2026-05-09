using Application.Common.Interfaces;
using Application.Features.Users;
using Application.Features.Users.List;
using Application.Test.Helpers;
using NSubstitute;
using Shared.Pagination;
using Shared.Results;

namespace Application.Test.Features.Users.List;

public class GetUsersQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryHandlerTests()
    {
        _unitOfWork = TestHelpers.CreateMockUnitOfWork();
        _currentUserService = TestHelpers.CreateMockCurrentUserService(1);
        _handler = new GetUsersQueryHandler(_unitOfWork, _currentUserService);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsPagedList()
    {
        // Arrange
        var userParams = new UserParams
        {
            PageNumber = 1,
            ItemsPerPage = 10
        };
        var query = new GetUsersQuery(userParams);
        var users = new List<UserDTO>
        {
            TestHelpers.CreateTestUserDto(1),
            TestHelpers.CreateTestUserDto(2)
        };
        var pagedList = new PagedList<UserDTO>(users, 2, 1, 10);

        _unitOfWork.ApplicationUserRepository.GetUsersDTOAsync(1, userParams)
            .Returns(pagedList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal(2, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_EmptyResult_ReturnsEmptyPagedList()
    {
        // Arrange
        var userParams = new UserParams
        {
            PageNumber = 1,
            ItemsPerPage = 10
        };
        var query = new GetUsersQuery(userParams);
        var pagedList = new PagedList<UserDTO>([], 0, 1, 10);

        _unitOfWork.ApplicationUserRepository.GetUsersDTOAsync(1, userParams)
            .Returns(pagedList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(0, result.Value.Count);
        Assert.Empty(result.Value.Items);
    }
}

