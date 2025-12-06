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

        unitOfWork.PostRepository.Add(post);
        await unitOfWork.CommitAsync(cancellationToken);
        return Result<ulong>.Created(post.Id);
    }
}
