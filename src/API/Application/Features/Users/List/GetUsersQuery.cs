namespace Application.Features.Users.List;

public record GetUsersQuery(UserParams UserParams) : IQuery<Result<PagedList<UserDTO>>>;

