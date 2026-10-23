using System.Text.Json;

namespace Notifications.API.Contracts;

/// <summary>Validates a <see cref="PushNotificationRequest"/> against the column limits of the stored notification.</summary>
public sealed class PushNotificationRequestValidator : AbstractValidator<PushNotificationRequest>
{
    public PushNotificationRequestValidator()
    {
        RuleFor(r => r.Source).NotEmpty().MaximumLength(NotificationLimits.SourceMaxLength);
        RuleFor(r => r.EventId).NotEmpty().MaximumLength(NotificationLimits.EventIdMaxLength);
        RuleFor(r => r.Type).NotEmpty().MaximumLength(NotificationLimits.TypeMaxLength);
        RuleFor(r => r.Title).NotEmpty().MaximumLength(NotificationLimits.TitleMaxLength);
        RuleFor(r => r.Body).MaximumLength(NotificationLimits.BodyMaxLength);
        RuleFor(r => r.Data).Must(BeValidJson).When(r => r.Data is not null).WithMessage("'Data' must be valid JSON.");
        RuleFor(r => r.RecipientIds).NotEmpty();
        RuleForEach(r => r.RecipientIds).NotEmpty().MaximumLength(NotificationLimits.RecipientIdMaxLength);
    }

    private static bool BeValidJson(string? json)
    {
        try
        {
            using var _ = JsonDocument.Parse(json!);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
