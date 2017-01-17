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
using OpenChat.Repositories;

namespace OpenChat.Communication
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public UserRepository UserRepository = new UserRepository();
        public RoomRepository RoomRepository = new RoomRepository();

        public ChatHub()
        {
            ConnectedUsers = new Dictionary<string, string>();
        }

        public Dictionary<string, string> ConnectedUsers { get; set; }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public async Task JoinRoom(Room room)
        {
            await Groups.Add(Context.ConnectionId, room.RoomName);
            RoomRepository.AddRoom(room);

            Clients.Group(room.RoomName).addChatMessage(Context.User.Identity.Name + " joined.");
        }

        public void SendMessageToGroup(string RoomName, string message)
        {
            var room = RoomRepository.Find(RoomName);
            string user = this.ConnectedUsers[Context.ConnectionId];
            RoomRepository.WriteMessage(message, user, RoomName);
        }

        public void Login(string username, string password)
        {
            if (!ConnectedUsers.ContainsKey(Context.ConnectionId))
            {
                if (UserRepository.LoginUser(username, password) == "404") return;
                ConnectedUsers.Add(Context.ConnectionId, username);
                Clients.Caller.Login(true);
            }
            else
            {
                Clients.Caller.Login(false);
            }
        }

        public void Register(string username, string password)
        {
            if (username != null || password != null)
            {
                this.UserRepository.AddUser(new User(username, password));
                Clients.Caller.Registered(true);
            }
            else
            {
                Clients.Caller.Registered(false);
            }
        }

        public void PublicSend(string message)
        {
            Clients.All.send(message);
        }

        public List<Room> LoadUserRooms(string username)
        {
            return RoomRepository.FindAllUserRooms(username);
        }
    }
}