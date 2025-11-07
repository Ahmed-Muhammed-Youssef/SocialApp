namespace Domain.ChatAggregate.Exceptions;

internal class InvalidMessageException(string reason) : DomainException($"Message is invalid: {reason}")
{
}
