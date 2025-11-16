namespace Domain.FriendAggregate.Exceptions;

public class InvalidFriendParametersException(string reason) : DomainException($"Parameters are invalid: {reason}")
{
}
