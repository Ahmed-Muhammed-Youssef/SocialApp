namespace Application.Features.Posts.GetByOwnerId;

public class GetPostsByOwnerIdHandler(IUnitOfWork _unitOfWork, ICurrentUserService currentUserService) : IQueryHandler<GetPostsByOwnerIdQuery, Result<IEnumerable<Post>>>
{
    public async ValueTask<Result<IEnumerable<Post>>> Handle(GetPostsByOwnerIdQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Post> posts = await _unitOfWork.PostRepository.GetUserPostsAsync(query.UserId, currentUserService.GetPublicId());

        return Result<IEnumerable<Post>>.Success(posts);
    }
}
