using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenChat.Models
{
    public class Room
    {
        public string RoomName { get; set; }
        public RoomType Type { get; set; }
        public List<ChatUser> Users { get; set; }
        public int ID { get; set; }
        public List<Message> Messages { get; set; }
    }

    public enum RoomType
    {
        Private,
        Public,
        Group
    };
}