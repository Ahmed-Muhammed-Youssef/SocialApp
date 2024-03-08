using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = [];
        public Task<bool> UserConnected(string username, string connectionId)
        {
            var firstConnection = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.TryGetValue(username, out List<string> userConnections))
                {
                    OnlineUsers[username] = [connectionId];
                    firstConnection = true;
                }
                else
                {
                    userConnections.Add(connectionId);
                }
            }
            return Task.FromResult(firstConnection);
        }
        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.TryGetValue(username, out List<string> userConnections))
                {
                    return Task.FromResult(isOffline);
                }

                userConnections.Remove(connectionId);
                if (userConnections.Count == 0)
                {
                    OnlineUsers.Remove(username);
                    isOffline = true;
                }
            }
            return Task.FromResult(isOffline);
        }
        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers = [];
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(u => u.Key).Select(u => u.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        }
        public Task<List<string>> GetConnectionForUser(string username)
        {
            List<string> connectionIds;
            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }
    }
}