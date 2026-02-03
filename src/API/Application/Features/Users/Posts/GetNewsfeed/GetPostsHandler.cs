
namespace Application.Features.Users.Posts.List;

public class GetPostsHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IQueryHandler<GetPostsQuery, Result<PagedList<PostDTO>>>
{
    public async ValueTask<Result<PagedList<PostDTO>>> Handle(GetPostsQuery query, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetPublicId();
        PagedList<PostDTO> posts = await unitOfWork.ApplicationUserRepository.GetNewsfeed(userId, query.PaginationParams, cancellationToken);

        return Result<PagedList<PostDTO>>.Success(posts);
    }
}
