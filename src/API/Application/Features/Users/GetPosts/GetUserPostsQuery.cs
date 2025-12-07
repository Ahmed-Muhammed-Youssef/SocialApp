namespace Application.Features.Users.GetPosts;

public record GetUserPostsQuery(int UserId) : IQuery<Result<List<PostDTO>>>;
