namespace Application.Features.Posts.Create;

public class CreatePostHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<CreatePostCommand, Result<ulong>>
{
    public async ValueTask<Result<ulong>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        Post post = new()
        {
            Content = command.Content,
            DatePosted = DateTime.UtcNow,
            UserId = currentUserService.GetPublicId()
        };

        await unitOfWork.PostRepository.AddAsync(post, cancellationToken);
        return Result<ulong>.Created(post.Id);
    }
}
