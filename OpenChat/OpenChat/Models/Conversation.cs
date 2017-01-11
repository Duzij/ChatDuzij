using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenChat.Models;

namespace OpenChatClient.Model
{
    public class Conversation
    {
        public bool GotNewMessages { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }

        public static explicit operator Conversation(ChatUser u)
        {
            return new Conversation() { RoomName = u.Username, GotNewMessages = false };
        }

        public static explicit operator Conversation(Room r)
        {
            return new Conversation() { RoomName = r.RoomName, RoomId = r.ID, GotNewMessages = false };
        }
    }
}