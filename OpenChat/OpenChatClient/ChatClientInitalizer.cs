using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace OpenChatClient
{
    public class ChatClientInitalizer
    {
        public ChatClientInitalizer(string server)
        {
            connection = new HubConnection(server);
            chat = connection.CreateHubProxy("Chat");
            connection.TraceLevel = TraceLevels.All;
            connection.TraceWriter = Console.Out;
        }

        public ChatClientInitalizer()
        {
            try
            {
                connection = new HubConnection(Server);
                chat = connection.CreateHubProxy("Chat");

                //client-side logging
                connection.TraceLevel = TraceLevels.All;
                connection.TraceWriter = Console.Out;
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
        }

        public string Server { get; set; }
        public HubConnection connection { get; set; }
        public IHubProxy chat { get; set; }

        public void Dispose()
        {
            this.connection.Dispose();
        }
    }
}