using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace OpenChatClient
{
    public class ChatClientInitalizer
    {
        public ChatClientInitalizer(string server)
        {
            connection = new HubConnection(server);
            chatProxy = connection.CreateHubProxy("Chat");
            connection.Closed += Connection_Closed;

            chatProxy.JsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            chatProxy.JsonSerializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

        }

        private void Connection_Closed()
        {
            if (connection != null)
                this.connection.Dispose();
        }

        public ChatClientInitalizer()
        {
            try
            {
                connection = new HubConnection(Server);
                chatProxy = connection.CreateHubProxy("chat");

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
        public IHubProxy chatProxy { get; set; }

        public void Dispose()
        {
            this.connection.Dispose();
        }
    }
}