using Application.Features.Messages;
using Application.Features.Messages.ConnectToChat;
using Application.Features.Messages.DisconnectFromChat;
using Application.Features.Messages.Send;

namespace API.SignalR;

public class MessageHub(IMediator mediator) : Hub
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
            await Groups.AddToGroupAsync(Context.ConnectionId, result.Value.Group.Name);
            await Clients.Group(result.Value.Group.Name).SendAsync("UpdatedGroup", result.Value.Group);
            await Clients.Caller.SendAsync("ReceiveMessages", result.Value.Messages);
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

        if (result.IsSuccess) {
            if (result.Value.Group is not null)
            {
                await Clients.Group(result.Value.Group.Name).SendAsync("UpdatedGroup", result.Value.Group);
            }
        }
        else
        {
            throw new HubException(string.Join('-', result.Errors));
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(NewMessageDTO message)
    {
        var cancellationToken = Context.ConnectionAborted;

        int currentUserId = Context.User?.GetPublicId()
           ?? throw new InvalidOperationException("Failed to resolve current user ID from claims.");

        var result = await mediator.Send(new SendMessageCommand(
            SenderId: currentUserId,
            RecipientId: message.RecipientId,
            Content: message.Content), cancellationToken);

        if(result.IsSuccess)
        {
            await Clients.Group(result.Value.GroupName).SendAsync("NewMessage", result.Value.MessageDTO);
        }
        else
        {
            throw new HubException(string.Join('-', result.Errors));
        }
    }
}