namespace Application.Features.FriendRequests.Delete;

public record DeleteFriendRequestCommand(int Id) : ICommand<Result<object?>>;
