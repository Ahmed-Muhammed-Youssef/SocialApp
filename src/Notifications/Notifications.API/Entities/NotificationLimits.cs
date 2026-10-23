namespace Notifications.API.Entities;

/// <summary>Column size limits, shared by the schema configuration and request validation.</summary>
public static class NotificationLimits
{
    public const int SourceMaxLength = 64;
    public const int EventIdMaxLength = 128;
    public const int TypeMaxLength = 64;
    public const int TitleMaxLength = 200;
    public const int BodyMaxLength = 2000;
    public const int RecipientIdMaxLength = 64;
}
