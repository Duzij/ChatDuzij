using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OpenChat.Models
{
    public class ChatUser
    {
        public ChatUser(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public ChatUser()
        {
        }

        public virtual List<Room> Rooms { get; } = new List<Room>();
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}