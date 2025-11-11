namespace Application.Features.FriendRequests.Accept;

public record AcceptFriendRequestCommand(int Id) : ICommand<Result<FriendCreatedResponse>>;
