using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatDuzijCore.Models.ChatModel
{
    public class Room
    {
        public List<ChatUser> Users { get; set; }
        public int ID { get; set; }
        public List<Message> Messages { get; set; }
    }
}