namespace Application.Features.Users.Posts.Create;

public class CreatePostHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<CreatePostCommand, Result<ulong>>
{
    public async ValueTask<Result<ulong>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        int userId = currentUserService.GetPublicId();

        ApplicationUser? user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            return Result<ulong>.NotFound("User not found.");
        }

        Post post = user.AddPost(command.Content);
        unitOfWork.ApplicationUserRepository.AddPost(post);

        await unitOfWork.CommitAsync(cancellationToken);
        return Result<ulong>.Created(post.Id);
    }
}
