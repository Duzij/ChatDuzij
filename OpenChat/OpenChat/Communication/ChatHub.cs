using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;
using ChatDuzijCore.Repositories;
using Microsoft.AspNet.SignalR;
using OpenChat.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace OpenChat.Communication
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public UserRepository UserRepository { get; set; }
        public ChatUser tempUser { get; set; }

        public override Task OnConnected()
        {
            UserRepository = new UserRepository();
            tempUser = new ChatUser() { Username = Context.QueryString["nick"] };
            
            return base.OnConnected();
        }

        public void Login()
        {
            Clients.Caller.JoinLobby();
            //UserRepository.AddUser(tempUser);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public void Send(string message)
        {
            Clients.All.send(message);
        }
    }
}