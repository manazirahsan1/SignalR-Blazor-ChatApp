using ConnectedUserChatContact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatContact
{
    public interface IChatClient
    {
        public Task ReceiveSystemMessage(string message);
        public Task UpdateUserList(List<ConnectedUser> users);
        public Task ReceiveMessage(string fromUserId, string fromConnectionId, string message);
    }
}
