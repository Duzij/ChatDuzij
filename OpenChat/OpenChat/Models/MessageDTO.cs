using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenChat.Models
{
    public class MessageDTO
    {
        public string Text { get; set; }
        public double Timestamp { get; set; }
        public string Author { get; set; }
        public string Room { get; set; }
        public bool MyMessage = false;

        public static explicit operator MessageDTO(Message m)
        {
            return new MessageDTO()
            {
                Author = m.Author,
                Room = m.Room,
                Text = String.Concat(m.Author, ": ", m.Text),
                Timestamp = m.Timestamp
            };
        }

    }
}