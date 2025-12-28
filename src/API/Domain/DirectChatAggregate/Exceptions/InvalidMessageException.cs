namespace Domain.DirectChatAggregate.Exceptions;

public class InvalidMessageException(string reason) : DomainException($"Message is invalid: {reason}")
{
}
