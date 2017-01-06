using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;
using ChatDuzijCore.Repositories;
using Microsoft.AspNet.SignalR;

namespace OpenChat.Communication
{
    public class ChatHub : Hub
    {
        public UserRepository UserRepository { get; set; }

        public override Task OnConnected()
        {
            //if (!UserRepository.LoginUser(Context.User.Identity.Name, Context.QueryString["pass"]))
            //    Clients.Caller.UILoginException();

            Console.WriteLine($"{Context.User.Identity.Name} is connected!");

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}