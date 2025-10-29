namespace Application.Features.Messages;

public static class ChatGroupHelper
{
    public static string GetGroupName(int callerId, int otherId) => 
        callerId > otherId 
        ? $"{callerId}-{otherId}" 
        : $"{otherId}-{callerId}";
}
