using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChatClient.Model
{
    public class Conversation
    {
        public bool GotNewMessages { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
    }
}