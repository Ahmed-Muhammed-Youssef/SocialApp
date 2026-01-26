namespace Application.Features.Users.Posts.List;

public record GetPostsQuery(PaginationParams PaginationParams) : IQuery<Result<PagedList<PostDTO>>>;
