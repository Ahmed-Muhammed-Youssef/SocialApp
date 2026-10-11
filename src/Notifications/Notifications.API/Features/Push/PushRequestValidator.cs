namespace Notifications.API.Features.Push;

public sealed class PushRequestValidator : AbstractValidator<PushRequest>
{
    public const int MaxRecipients = 1000;

    public PushRequestValidator(TimeProvider timeProvider)
    {
        RuleFor(r => r.EventId).NotEmpty().MaximumLength(128);
        RuleFor(r => r.Title).NotEmpty().MaximumLength(200);
        RuleFor(r => r.Body).MaximumLength(2000);

        RuleFor(r => r.Recipients)
            .NotEmpty()
            .Must(recipients => recipients.Count <= MaxRecipients)
            .WithMessage($"A push can target at most {MaxRecipients} recipients.");
        RuleForEach(r => r.Recipients).NotEmpty().MaximumLength(64);

        RuleFor(r => r.ExpiresAt)
            .Must(expiresAt => expiresAt is null || expiresAt > timeProvider.GetUtcNow())
            .WithMessage("ExpiresAt must be in the future.");
    }
}
