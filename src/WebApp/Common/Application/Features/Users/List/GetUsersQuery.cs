using Application.DTOs.Pagination;
using Mediator;
using Shared.Results;

namespace Application.Features.Users.List;

public record GetUsersQuery(UserParams UserParams) : IQuery<Result<PagedList<UserDTO>>>;

