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
using OpenChatClient.Models;
using WebGrease.Css.Extensions;

namespace OpenChat.Communication
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public UserRepository UserRepository = new UserRepository();
        public RoomRepository RoomRepository = new RoomRepository();

        public ChatHub()
        {
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
                this.UserRepository.RemoveIndentity(this.Context.ConnectionId);
            else
            {
            }
            return base.OnDisconnected(stopCalled);
        }

        public List<UserDTO> LoadUsers(string usename)
        {
            return UserRepository.FindAll().ConvertAll(a => (UserDTO)a);
        }

        public void LoadRooms(string username)
        {
            Clients.Caller.LoadRooms(RoomRepository.FindAllUserRooms(username).ConvertAll(a => (RoomDTO)a));
        }

        public void LoadRoomMessages(string roomName, string authorName)
        {
            List<MessageDTO> output = new List<MessageDTO>();
            var list = RoomRepository.GetAllMessages(roomName);
            if (list.Count != 0)
            {
                foreach (var msg in list)
                {
                    var msgDTO = new MessageDTO();
                    msgDTO = (MessageDTO)msg;
                    if (authorName == msg.Author)
                        msgDTO.MyMessage = true;
                    output.Add(msgDTO);
                }
            }
            Clients.Caller.LoadRoomMessages(output);
        }

        public void SendMessage(string RoomName, string message, string user)
        {
            RoomRepository.WriteMessage(message, user, RoomName);
            if (UserRepository.GetAllIdentities() != null)
                Clients.Group(RoomName).Notify(RoomName);
        }

        public void CreateRoom(CreateRoomDTO room)
        {
            var roomName = room.Name;
            var addedUsers = room.Users;
            if (RoomRepository.Find(roomName) == null)
            {
                RoomRepository.AddRoom(roomName);

                foreach (var user in addedUsers)
                {
                    RoomRepository.JoinRoom(user, roomName);

                    if (UserRepository.IsUserConnected(user))
                    {
                        JoinRoom(roomName, user);
                        Clients.Group(roomName).LoadRooms(RoomRepository.FindAllUserRooms(user).ConvertAll(a => (RoomDTO)a));
                    }
                }
            }
        }

        public void JoinRoom(string roomName, string username)
        {
            //what meens, he is online
            Groups.Add(UserRepository.GetUserConnectionByUsername(username), roomName);
        }

        public void Login(string username, string password)
        {
            if (!UserRepository.IsUserConnected(username) && UserRepository.Exist(username))
            {
                if (UserRepository.LoginUser(username, password) == "404")
                {
                    //register a new user instantly
                    UserRepository.AddUser(new User(username, password));
                }

                UserRepository.AddIdentity(username, Context.ConnectionId);

                if (UserRepository.Find(username).Rooms != null)
                {
                    foreach (var room in RoomRepository.FindAllUserRooms(username))
                    {
                        JoinRoom(room.RoomName, username);
                    }
                }

                Clients.Caller.Login(true);
            }
            else
            {
                Clients.Caller.Login(false);
            }
        }
    }
}