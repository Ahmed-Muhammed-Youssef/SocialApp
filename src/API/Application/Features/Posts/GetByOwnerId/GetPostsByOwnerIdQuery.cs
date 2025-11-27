namespace Application.Features.Posts.GetByOwnerId;

public record GetPostsByOwnerIdQuery(int UserId) : IQuery<Result<List<PostDTO>>>;
