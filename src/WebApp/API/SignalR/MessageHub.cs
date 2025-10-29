using Application.Common.Interfaces;
using Application.Features.Messages;
using Application.Features.Messages.Send;

namespace API.SignalR;

public class MessageHub(IUnitOfWork unitOfWork, IMediator mediator) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var cancellationToken = Context.ConnectionAborted;

        // Validate the user identity first
        int currentUserId = Context.User?.GetPublicId()
            ?? throw new InvalidOperationException("Failed to resolve current user ID from claims.");

        HttpContext httpContext = Context.GetHttpContext()
            ?? throw new InvalidOperationException("Failed to get HttpContext from SignalR context.");

        // Try to parse the "userId" query string
        if (!httpContext.Request.Query.TryGetValue("userId", out var userIdValues) ||
            !int.TryParse(userIdValues.FirstOrDefault(), out int otherUserId))
        {
            throw new InvalidOperationException("Missing or invalid 'userId' query parameter.");
        }

        if (currentUserId == otherUserId)
        {
            throw new InvalidOperationException("Cannot start a chat with yourself.");
        }

        string groupName = GetGroupName(httpContext.User.GetPublicId(), otherUserId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        Group group = await AddToGroup(groupName, cancellationToken);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        IEnumerable<MessageDTO> messages = await unitOfWork.MessageRepository.GetMessagesDTOThreadAsync(currentUserId, otherUserId, cancellationToken);

        if (unitOfWork.HasChanges())
        {
            await unitOfWork.SaveChangesAsync();
        }
        
        await Clients.Caller.SendAsync("ReceiveMessages", messages);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var cancellationToken = Context.ConnectionAborted;
        Group? group = await RemoveFromMessageGroup(cancellationToken);

        if(group is not null)
        {
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
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

    // utility methods
    private async Task<Group> AddToGroup(string groupName, CancellationToken cancellationToken)
    {
        try
        {
            var group = await unitOfWork.GroupRepository.GetGroupByName(groupName, cancellationToken);
            var connection = new Connection(Context.ConnectionId, Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id"));
            if (group == null)
            {
                group = new Group(name: groupName);
                group.Connections.Add(connection);
                await unitOfWork.GroupRepository.AddAsync(group, cancellationToken);
            }

            return group;
        }
        catch (Exception ex)
        {
            throw new HubException("Failed to create group", ex);
        }
    }
    private async Task<Group?> RemoveFromMessageGroup(CancellationToken cancellationToken)
    {
        try
        {
            Group? group = await unitOfWork.GroupRepository.GetGroupByConnectionId(Context.ConnectionId, cancellationToken);

            if(group is null) return null;

            Connection? connection = group.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            if(connection is not null)
            {
                await unitOfWork.ConnectionRepository.DeleteAsync(connection, cancellationToken);
            }

            return group;
        }
        catch (Exception ex)
        {
            throw new HubException("Failed to remove group", ex);
        }
    }
    private static string GetGroupName(int callerId, int otherId) => callerId > otherId ? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";
}