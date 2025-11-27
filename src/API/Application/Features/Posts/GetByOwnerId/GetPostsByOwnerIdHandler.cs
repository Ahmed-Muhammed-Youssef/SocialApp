namespace Application.Features.Posts.GetByOwnerId;

public class GetPostsByOwnerIdHandler(IUnitOfWork _unitOfWork, ICurrentUserService currentUserService) : IQueryHandler<GetPostsByOwnerIdQuery, Result<List<PostDTO>>>
{
    public async ValueTask<Result<List<PostDTO>>> Handle(GetPostsByOwnerIdQuery query, CancellationToken cancellationToken)
    {
        List<PostDTO> posts = await _unitOfWork.PostRepository.GetUserPostsAsync(query.UserId, currentUserService.GetPublicId());

        return Result<List<PostDTO>>.Success(posts);
    }
}
