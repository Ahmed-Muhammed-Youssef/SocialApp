using Application.DTOs.User;
using Application.Interfaces;
using Infrastructure.RealTime.Presence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;

namespace MVC.Hubs
{
    [Authorize]
    public class PresenceHub(OnlinePresenceManager presenceManager, IUnitOfWork unitOfWork) : Hub
    {
        private readonly OnlinePresenceManager _presenceManager = presenceManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public override async Task OnConnectedAsync()
        {
            int publicId = Context.User.GetPublicId().Value;

            var isFirstConnection = await _presenceManager.UserConnected(publicId, Context.ConnectionId);
            if (isFirstConnection)
            {
                SimplifiedUserDTO connectedUser = await _unitOfWork.ApplicationUserRepository.GetSimplifiedDTOAsync(publicId);
                await Clients.Others.SendAsync("UserIsOnline", connectedUser);
            }

            int[] onlineUsersIds = await _presenceManager.GetOnlineUsers();

            List<SimplifiedUserDTO> onlineUsers = await _unitOfWork.ApplicationUserRepository.GetListAsync(onlineUsersIds.Where(id => id != publicId).ToArray());
            await Clients.Caller.SendAsync("GetOnlineUsers", onlineUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            int publicId = Context.User.GetPublicId().Value;
            var isJustDisconnected = await _presenceManager.UserDisconnected(publicId, Context.ConnectionId);
            if (isJustDisconnected)
            {
                await Clients.Others.SendAsync("UserIsOffline", publicId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
