namespace Application.Features.Users.Posts.GetById;

public record GetPostByIdQuery(ulong PostId) : IQuery<Result<Post>>;
