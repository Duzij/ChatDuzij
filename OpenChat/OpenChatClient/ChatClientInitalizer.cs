using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChatClient
{
    public class ChatClientInitalizer 
    {
        internal HubConnection mychat;

        public string Server { get; set; }

        public ChatClientInitalizer(string server)
        {
            connection = new HubConnection(server);
            chat = connection.CreateHubProxy("Chat");
        }

        public ChatClientInitalizer()
        {
            try
            {
                connection = new HubConnection(Server);
                chat = connection.CreateHubProxy("Chat");
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
        }

        public HubConnection connection { get; set; }
        public IHubProxy chat { get; set; }

        public void Dispose()
        {
            this.connection.Dispose();
        }
    }
}
