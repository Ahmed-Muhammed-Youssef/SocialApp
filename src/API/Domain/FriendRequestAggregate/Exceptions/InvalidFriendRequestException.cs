namespace Domain.FriendRequestAggregate.Exceptions;

public class InvalidFriendRequestException(string reason) : DomainException($"Friend request is invalid: {reason}")
{
}
