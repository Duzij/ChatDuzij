using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OpenChat.Models
{
    public class User
    {
        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public User()
        {
        }

        public virtual List<Room> Rooms { get; } = new List<Room>();

        [Key]
        public string Username { get; set; }

        public string Password { get; set; }
    }
}