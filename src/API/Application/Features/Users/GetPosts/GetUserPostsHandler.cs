namespace Application.Features.Users.GetPosts;

public class GetUserPostsHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUserPostsQuery, Result<List<PostDTO>>>
{
    public async ValueTask<Result<List<PostDTO>>> Handle(GetUserPostsQuery query, CancellationToken cancellationToken)
    {
        List<PostDTO> posts = await unitOfWork.ApplicationUserRepository.GetUserPostsAsync(query.UserId, cancellationToken);

        return Result<List<PostDTO>>.Success(posts);
    }
}
