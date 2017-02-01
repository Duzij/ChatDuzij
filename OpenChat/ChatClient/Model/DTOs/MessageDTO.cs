using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenChatClient.Models
{
    public class MessageDTO
    {
        private bool myMessage;
        public string Text { get; set; }
        public double Timestamp { get; set; }
        public string Author { get; set; }
        public string Room { get; set; }

        public virtual bool MyMessage
        {
            get { return myMessage; }
            set { myMessage = value; }
        }

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