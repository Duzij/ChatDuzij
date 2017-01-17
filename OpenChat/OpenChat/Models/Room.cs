using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenChat.Models
{
    public class Room
    {
        public Room(string roomName)
        {
            this.RoomName = roomName;
        }

        public Room()
        {
        }

        public virtual List<Message> Messages { get; } = new List<Message>();

        [Key]
        public string RoomName { get; set; }

        public virtual List<User> Users { get; } = new List<User>();
    }
}