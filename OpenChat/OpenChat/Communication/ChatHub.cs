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
using OpenChatClient.Model;

namespace OpenChat.Communication
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public UserRepository UserRepository { get; set; }
        public RoomRepository RoomRepository { get; set; }
        private ChatUser _tempUser { get; set; }

        public override Task OnConnected()
        {
            UserRepository = new UserRepository();
            _tempUser = new ChatUser() { Username = Context.QueryString["nick"] };

            return base.OnConnected();
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        public void Login(string username, string password)
        {
            var id = UserRepository.LoginUser(username, password);
            if (id != 0)
            {
                this.LoadUserRooms(id);
                this.LoadUserContacts(id);
            }
            else
                UserRepository.AddUser(_tempUser);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public void Send(string message)
        {
            Clients.All.send(message);
            Clients.Caller.login();
            UserRepository.AddUser(_tempUser);
        }

        public List<Conversation> LoadUserRooms(int id)
        {
            return RoomRepository.FindAllUserRooms(id).ConvertAll(a => (Conversation)a);
        }

        public List<Conversation> LoadUserContacts(int id)
        {
            return UserRepository.FindAllUserContacts(id).ConvertAll(a => (Conversation)a);
        }

        public List<Message> LoadRoom(int roomId)
        {
            return RoomRepository.FindById(roomId).Messages;
        }
    }
}