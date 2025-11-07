namespace Domain.ChatAggregate.Exceptions;

public class InvalidSenderException(string reason) : DomainException($"Sender is invalid: {reason}")
{
}
