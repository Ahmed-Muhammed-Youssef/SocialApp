using Application.Common.Interfaces;
using Application.Features.Users.Posts.Create;
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
        var user = new ApplicationUser("John", "Doe", DateTime.UtcNow.AddYears(-25), Gender.Male, 1);
        user.AssociateWithIdentity("test-identity");

        // Set the Id property (normally set by EF Core)
        typeof(ApplicationUser).GetProperty("Id")!.SetValue(user, 1);

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(user);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Created, result.Status);

        _unitOfWork.ApplicationUserRepository.Received(1).AddPost(Arg.Is<Post>(p =>
            p.Content == command.Content &&
            p.UserId == 1));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_EmptyContent_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreatePostCommand("");
        var user = new ApplicationUser("John", "Doe", DateTime.UtcNow.AddYears(-25), Gender.Male, 1);
        user.AssociateWithIdentity("test-identity");

        _unitOfWork.ApplicationUserRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(user);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
    }
}

