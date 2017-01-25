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
using WebGrease.Css.Extensions;

namespace OpenChat.Communication
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public UserRepository UserRepository = new UserRepository();
        public RoomRepository RoomRepository = new RoomRepository();

        public Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();

        public ChatHub()
        {
        }

        public List<UserDTO> LoadUsers(string usename)
        {
            return UserRepository.FindAll().Where(a => a.Username != usename).ToList().ConvertAll(a => (UserDTO)a);
        }

        public void LoadRooms(string username)
        {
            Clients.Caller.LoadRooms(RoomRepository.FindAllUserRooms(username).ConvertAll(a => (RoomDTO)a));
        }

        public void LoadRoomsForRoom(string roomName)
        {
            Clients.Group(roomName).LoadRooms();
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
            Clients.Group(RoomName).Notify(RoomName);
        }

        public void CreateRoom(string roomName, List<string> addedUsers)
        {
            RoomRepository.AddRoom(roomName);
            foreach (var user in addedUsers)
            {
                if (ConnectedUsers.ContainsKey(user))
                {
                    JoinRoom(roomName, user);
                    Clients.Client(ConnectedUsers[user]).LoadRooms(user);
                }
            }
        }

        public void JoinRoom(string roomName, string username)
        {
            Groups.Add(ConnectedUsers[username], roomName);
            RoomRepository.JoinRoom(username, roomName);
        }

        public void Login(string username, string password)
        {
            if (!ConnectedUsers.ContainsValue(Context.ConnectionId) && !ConnectedUsers.ContainsKey(username))
            {
                if (UserRepository.LoginUser(username, password) == "404") return;

                ConnectedUsers.Add(username, Context.ConnectionId);

                foreach (var room in RoomRepository.FindAllUserRooms(username))
                {
                    JoinRoom(room.RoomName, username);
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