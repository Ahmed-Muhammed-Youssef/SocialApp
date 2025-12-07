namespace Application.Features.Posts.List;

public record GetPostsQuery(PaginationParams PaginationParams) : IQuery<Result<PagedList<PostDTO>>>;
