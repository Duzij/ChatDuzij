using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatDuzijCore.Models.ChatModel
{
    public class Message
    {
        public Message()
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            this.Timestamp = (DateTime.Today - dtDateTime).TotalSeconds;
        }

        public int ID { get; set; }
        public string Text { get; set; }
        public double Timestamp { get; set; }
        public ChatUser Author { get; set; }
        public Room Room { get; set; }
    }
}