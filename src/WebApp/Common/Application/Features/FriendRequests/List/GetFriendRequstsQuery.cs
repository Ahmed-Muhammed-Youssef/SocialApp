using Application.Features.Users;
using Mediator;
using Shared.Results;

namespace Application.Features.FriendRequests.List;

public record GetFriendRequstsQuery() : IQuery<Result<List<UserDTO>>>;
