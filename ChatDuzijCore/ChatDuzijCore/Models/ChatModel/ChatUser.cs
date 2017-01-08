using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Swashbuckle.Swagger;

namespace ChatDuzijCore.Models.ChatModel
{
    public class ChatUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IEnumerable<ChatUser> Contacts { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
    }
}