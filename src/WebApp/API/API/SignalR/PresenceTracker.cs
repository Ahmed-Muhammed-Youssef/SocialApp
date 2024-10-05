using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<int, List<string>> OnlineUsers = [];
        public Task<bool> UserConnected(int userId, string connectionId)
        {
            var firstConnection = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.TryGetValue(userId, out List<string> userConnections))
                {
                    OnlineUsers[userId] = [connectionId];
                    firstConnection = true;
                }
                else
                {
                    userConnections.Add(connectionId);
                }
            }
            return Task.FromResult(firstConnection);
        }
        public Task<bool> UserDisconnected(int userId, string connectionId)
        {
            bool isOffline = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.TryGetValue(userId, out List<string> userConnections))
                {
                    return Task.FromResult(isOffline);
                }

                userConnections.Remove(connectionId);
                if (userConnections.Count == 0)
                {
                    OnlineUsers.Remove(userId);
                    isOffline = true;
                }
            }
            return Task.FromResult(isOffline);
        }
        public Task<int[]> GetOnlineUsers()
        {
            int[] onlineUsers = [];
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(u => u.Key).Select(u => u.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        }
        public Task<List<string>> GetConnectionForUser(int userId)
        {
            List<string> connectionIds;
            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(userId);
            }

            return Task.FromResult(connectionIds);
        }
    }
}