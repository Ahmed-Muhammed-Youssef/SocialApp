namespace Application.Features.FriendRequests.List;

public record GetFriendRequstsQuery() : IQuery<Result<List<UserDTO>>>;
