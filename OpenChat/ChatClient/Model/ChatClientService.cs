using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using ChatClient.Model;
using OpenChatClient.Models;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Ioc;
using OpenChatClient;

namespace OpenChatClient
{
    public class ChatClientSevice : IChatClientService
    {
        [PreferredConstructor]
        public ChatClientSevice(string server)
        {
            try
            {
                connection = new HubConnection(server);
                chatProxy = connection.CreateHubProxy("Chat");
                connection.Closed += Connection_Closed;

                connection.TraceLevel = TraceLevels.All;
                connection.TraceWriter = Console.Out;
                chatProxy.JsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                chatProxy.JsonSerializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public ChatClientSevice()
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

        public List<RoomDTO> LoadedRooms { get; set; }

        public string Server { get; set; }

        public HubConnection connection { get; set; }

        public IHubProxy chatProxy { get; set; }

        public void Dispose()
        {
            this.connection.Dispose();
        }

        private void Connection_Closed()
        {
            if (connection != null)
                this.connection.Dispose();
        }
    }
}