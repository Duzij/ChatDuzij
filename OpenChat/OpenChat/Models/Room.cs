using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenChat.Models
{
    public enum RoomType
    {
        Private,
        Public,
        Group
    };

    public class Room
    {
        public Room(string roomName, RoomType type)
        {
            this.RoomName = roomName;
            this.Type = type;
        }

        public Room()
        {
        }

        public virtual List<Message> Messages { get; } = new List<Message>();
        public string RoomName { get; set; }
        public RoomType Type { get; set; }

        public virtual List<ChatUser> Users { get; } = new List<ChatUser>();
        public int ID { get; set; }
    }
}