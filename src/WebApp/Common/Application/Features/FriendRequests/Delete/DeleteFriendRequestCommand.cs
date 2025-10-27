using Mediator;
using Shared.Results;

namespace Application.Features.FriendRequests.Delete;

public record DeleteFriendRequestCommand(int UserId) : ICommand<Result<object?>>;
