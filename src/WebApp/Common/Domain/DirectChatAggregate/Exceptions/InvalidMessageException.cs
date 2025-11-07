namespace Domain.DirectChatAggregate.Exceptions;

internal class InvalidMessageException(string reason) : DomainException($"Message is invalid: {reason}")
{
}
