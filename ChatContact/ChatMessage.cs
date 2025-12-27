using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatContact
{
    public class ChatMessage
    {
        public string? FromUserId { set; get; }
        public string? ToUserId { set; get; }
        public string? Message { set; get; }
        public bool Unread { set; get; } = true;
    }
}
