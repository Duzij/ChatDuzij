using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenChat.Models;

namespace OpenChatClient.Model
{
    public class RoomDTO
    {
        public bool GotNewMessages { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public RoomType Type { get; set; }

        public static explicit operator RoomDTO(ChatUser u)
        {
            return new RoomDTO() { Type = RoomType.Private, RoomName = u.Username, GotNewMessages = false };
        }

        public static explicit operator RoomDTO(Room r)
        {
            return new RoomDTO() { Type = RoomType.Group, RoomName = r.RoomName, RoomId = r.ID, GotNewMessages = false };
        }
    }
}