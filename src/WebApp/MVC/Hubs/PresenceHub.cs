using Application.Common.Interfaces;
using Application.Features.Users;
using Infrastructure.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;

namespace MVC.Hubs;

[Authorize]
public class PresenceHub(OnlineUsersStore presenceManager, IUnitOfWork unitOfWork) : Hub
{
    private readonly OnlineUsersStore _presenceManager = presenceManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task OnConnectedAsync()
    {
        int publicId = Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id");

        var isFirstConnection = await _presenceManager.AddUserConnection(publicId, Context.ConnectionId);
        if (isFirstConnection)
        {
            SimplifiedUserDTO? connectedUser = await _unitOfWork.ApplicationUserRepository.GetSimplifiedDTOAsync(publicId);
            await Clients.Others.SendAsync("UserIsOnline", connectedUser);
        }

        int[] onlineUsersIds = await _presenceManager.GetOnlineUsers();

        List<SimplifiedUserDTO> onlineUsers = await _unitOfWork.ApplicationUserRepository.GetListAsync(onlineUsersIds.Where(id => id != publicId).ToArray());
        await Clients.Caller.SendAsync("GetOnlineUsers", onlineUsers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        int publicId = Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id");
        var isJustDisconnected = await _presenceManager.RemoveUserConnection(publicId, Context.ConnectionId);
        if (isJustDisconnected)
        {
            await Clients.Others.SendAsync("UserIsOffline", publicId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
