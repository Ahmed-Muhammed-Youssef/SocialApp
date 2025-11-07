namespace Application.Features.FriendRequests.Create;

public record CreateFriendRequestCommand(int UserId) : ICommand<Result<int>>;
