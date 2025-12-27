using ChatContact;
using ConnectedUserChatContact;
using Microsoft.AspNetCore.SignalR;

namespace HubServer
{
    public class ChatHub: Hub<IChatClient>
    {
        private static readonly object _lockUser = new object();
        private static List<ConnectedUser> _connectedUsers = new List<ConnectedUser>();
        public override async Task OnConnectedAsync()
        {
            var userId = string.Empty;
            var userName = string.Empty;
            userId = Context.GetHttpContext()?.Request.Query["userId"];
            userName = Context.GetHttpContext()?.Request.Query["userName"];

            lock (_lockUser)
            {
                _connectedUsers.Add(new ConnectedUser
                {
                    UserId = userId,
                    UserName = userName,
                    ConnectionId = Context.ConnectionId
                });
            }
            await Clients.Caller.ReceiveSystemMessage($"Hi {userName}, your connection ID is {userId} and connecting string is {Context.ConnectionId}");
            await Clients.All.UpdateUserList(_connectedUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exceptoin)
        {
            ConnectedUser? user;
            lock (_lockUser)
            {
                user = _connectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
                if (user != null)
                {
                    _connectedUsers.Remove(user);
                }
            }
            if (user != null)
            {
                // User connected user list at client side
                await Clients.All.UpdateUserList(_connectedUsers);
            }
            await base.OnDisconnectedAsync(exceptoin);
        }
        public async Task ForwardMessage(string fromUserId, string ConnectionId, string message)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionId))
            {
                await Clients.Client(ConnectionId).ReceiveMessage(fromUserId, Context.ConnectionId, message);
            }
        }
    }
}


