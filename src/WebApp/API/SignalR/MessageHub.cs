using Application.Common.Interfaces;
using Application.Features.Messages;

namespace API.SignalR;

public class MessageHub(IUnitOfWork _unitOfWork, IMapper _mapper,
    IHubContext<PresenceHub> _presenceHubContext, OnlinePresenceManager _presenceTracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
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

        Group group = await AddToGroup(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        IEnumerable<MessageDTO> messages = await _unitOfWork.MessageRepository.GetMessagesDTOThreadAsync(currentUserId, otherUserId);

        if (_unitOfWork.HasChanges())
        {
            await _unitOfWork.SaveChangesAsync();
        }
        
        await Clients.Caller.SendAsync("ReceiveMessages", messages);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Group? group = await RemoveFromMessageGroup();

        if(group is not null)
        {
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
        }

        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessages(NewMessageDTO message)
    {
        int currentUserId = Context.User?.GetPublicId()
           ?? throw new InvalidOperationException("Failed to resolve current user ID from claims.");

        var sender = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(currentUserId);

        var recipient = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(message.RecipientId);

        if (recipient == null || sender == null)
        {
            throw new HubException("User not found");
        }
        if (sender.Id == recipient.Id)
        {
            throw new HubException("You can't send messages to yourself");
        }
        if (!await _unitOfWork.FriendRequestRepository.IsFriend(sender.Id, recipient.Id))
        {
            throw new HubException("You can't send messages to an unmatch");

        }
        var createdMessage = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = message.Content,
            SenderDeleted = false,
            RecipientDeleted = false,
            ReadDate = null
        };
        

        // @ToDo: Add Profile Picture Here
        // msgDTO.SenderPhotoUrl = null;
        var groupName = GetGroupName(sender.Id, recipient.Id);
        var group = await _unitOfWork.MessageRepository.GetGroupByName(groupName);

        if (group is not null && group.Connections.Any(c => c.UserId == recipient.Id))
        {
            createdMessage.ReadDate = DateTime.UtcNow;
        }
        else
        {
            var recipientConnections = await _presenceTracker.GetConnectionForUser(recipient.Id);
            if (recipientConnections != null)
            {
                var senderDTO = _mapper.Map<UserDTO>(sender);
                var msgDTO = _mapper.Map<MessageDTO>(createdMessage);
                await _presenceHubContext.Clients.Clients(recipientConnections)
                .SendAsync("NewMessage", new { senderDTO, msgDTO });
            }
        }

        try
        {
            await _unitOfWork.MessageRepository.AddMessageAsync(createdMessage);
            await _unitOfWork.SaveChangesAsync();
            

            await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDTO>(createdMessage));
        }
        catch(Exception ex) 
        { 
            throw new HubException("Couldn't Send the message", ex);
        }
    }

    // utility methods
    private async Task<Group> AddToGroup(string groupName)
    {
        var group = await _unitOfWork.MessageRepository.GetGroupByName(groupName);
        var connection = new Connection(Context.ConnectionId, Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id"));

        if (group == null)
        {
            group = new Group(name: groupName);
            await _unitOfWork.MessageRepository.AddGroupAsync(group);
        }

        try
        {
            group.Connections.Add(connection);
            await _unitOfWork.SaveChangesAsync();
            return group;

        }
        catch (Exception ex)
        {

            throw new HubException("Failed to create group", ex);
        }
    }
    private async Task<Group?> RemoveFromMessageGroup()
    {
        try
        {
            Group? group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);

            if(group is null) return null;

            Connection? connection = group.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            if(connection is not null)
            {
                _unitOfWork.MessageRepository.RemoveConnection(connection);
            }
            await _unitOfWork.SaveChangesAsync();

            return group;
        }
        catch (Exception ex)
        {
            throw new HubException("Failed to remove group", ex);
        }
    }
    private static string GetGroupName(int callerId, int otherId) => callerId > otherId ? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";

}