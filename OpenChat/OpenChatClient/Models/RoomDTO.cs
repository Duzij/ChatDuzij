using OpenChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenChatClient.Models
{
    public class RoomDTO
    {
        public bool GotNewMessages { get; set; }
        public string RoomName { get; set; }

        public static explicit operator RoomDTO(Room r)
        {
            return new RoomDTO() { RoomName = r.RoomName, GotNewMessages = false };
        }
    }
}