namespace Application.Features.Posts.GetById;

public record GetPostByIdQuery(ulong PostId) : IQuery<Result<Post>>;
