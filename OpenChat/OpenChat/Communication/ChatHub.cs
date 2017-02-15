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

        /// <summary>
        /// Removes username from database table, where all online users are
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
                this.UserRepository.RemoveIndentity(this.Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Returns user contacts by username.
        /// </summary>
        /// <param name="usename"></param>
        /// <returns></returns>
        public List<UserDTO> LoadUsers(string usename)
        {
            return UserRepository.FindAll().ConvertAll(a => (UserDTO)a);
        }

        /// <summary>
        /// Returns user rooms by username.
        /// </summary>
        /// <param name="username"></param>
        public void LoadRooms(string username)
        {
            Clients.Caller.LoadRooms(RoomRepository.FindAllUserRooms(username).ConvertAll(a => (RoomDTO)a));
        }

        /// <summary>
        /// Load messages and by authorName marks every message if usernames are same.
        /// This mark is userd for aligning messages by converter.
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="authorName"></param>
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

        /// <summary>
        /// Removes user from room and notify him
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="userName"></param>
        public void LeaveRoom(string roomName, string userName)
        {
            this.RoomRepository.LeaveRoom(roomName, userName);
            Clients.Caller.LeaveRoom(roomName);
        }

        /// <summary>
        /// Message is added to database and notify all online users.
        /// </summary>
        /// <param name="RoomName"></param>
        /// <param name="message"></param>
        /// <param name="user"></param>
        public void SendMessage(string RoomName, string message, string user)
        {
            RoomRepository.WriteMessage(message, user, RoomName);
            if (UserRepository.GetAllIdentities().Count != 0)
                Clients.Group(RoomName).Notify(RoomName);
        }

        /// <summary>
        /// Room is added to database by DTO and reloades room list for all online users
        /// </summary>
        /// <param name="room"></param>
        public void CreateRoom(CreateRoomDTO room)
        {
            var roomName = room.Name;
            if (RoomRepository.Find(roomName) == null)
            {
                RoomRepository.AddRoom(roomName);

                foreach (var user in room.Users)
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

        /// <summary>
        /// For all uses, which are online, signalR creates groups
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="username"></param>
        public void JoinRoom(string roomName, string username)
        {
            Groups.Add(UserRepository.GetUserConnectionByUsername(username), roomName);
        }

        /// <summary>
        /// This method provides basic login
        /// If username isnt found, new user is created.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Login(string username, string password)
        {
            Clients.All.Send(username + " " + Context.ConnectionId);
            if (!UserRepository.IsUserConnected(username) && UserRepository.Exist(username))
            {
                if (UserRepository.LoginUser(username, password) == "404")
                    UserRepository.AddUser(new User(username, password));

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