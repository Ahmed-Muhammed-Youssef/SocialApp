using NuGet.Protocol.Plugins;

namespace API.Features.DirectChats;

public class ChatHub(IMediator mediator, IMessageNotifier messageNotifier) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var cancellationToken = Context.ConnectionAborted;

        // Validate the user identity first
        int currentUserId = Context.User?.GetPublicId()
            ?? throw new HubException("Failed to resolve current user ID from claims.");

        HttpContext httpContext = Context.GetHttpContext()
            ?? throw new HubException("Failed to get HttpContext from SignalR context.");

        // Try to parse the "userId" query string
        if (!httpContext.Request.Query.TryGetValue("userId", out var userIdValues) ||
            !int.TryParse(userIdValues.FirstOrDefault(), out int otherUserId))
        {
            throw new HubException("Missing or invalid 'userId' query parameter.");
        }

        Result<ConnectToChatResult> result = await mediator.Send(new ConnectToChatCommand(
            ConnectionId: Context.ConnectionId,
            CurrentUserId: currentUserId,
            OtherUserId: otherUserId), cancellationToken);

        if(result.IsSuccess)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, result.Value.Group.Name, cancellationToken);
            await Clients.Caller.SendAsync("ReceiveMessages", result.Value.Messages, cancellationToken);
        }
        else
        {
            throw new HubException(string.Join('-', result.Errors));
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var cancellationToken = Context.ConnectionAborted;
        var result = await mediator.Send(new DisconnectFromChatCommand(ConnectionId: Context.ConnectionId), cancellationToken);

        if (!result.IsSuccess) {
            
            throw new HubException(string.Join('-', result.Errors));
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(SendMessageRequest request)
    {
        var cancellationToken = Context.ConnectionAborted;

        int currentUserId = Context.User?.GetPublicId()
           ?? throw new InvalidOperationException("Failed to resolve current user ID from claims.");

        var result = await mediator.Send(new SendMessageCommand(
            SenderId: currentUserId,
            RecipientId: request.RecipientId,
            Content: request.Content), cancellationToken);

        if(result.IsSuccess)
        {
            await Clients.Group(result.Value.GroupName).SendAsync("NewMessage", result.Value.MessageDTO, cancellationToken);
            await messageNotifier.NotifyRecipientAsync(result.Value.UserDTO, result.Value.MessageDTO, cancellationToken);
        }
        else
        {
            throw new HubException(string.Join('-', result.Errors));
        }
    }
}