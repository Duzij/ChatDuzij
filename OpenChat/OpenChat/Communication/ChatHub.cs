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
        //todo method, which loads last messages by room id
        //todo method, which get last message from Room by roomID and lastMsgID (to do not send the whole msg pack at once)
        //
        //todo method, which creates private user room, if it waas not created earlier (nameConvence => userID + opponentID)
        //
        //user double clicks on contats section => if (roomRepo.Find(userID+opponentID) = null =>
        //it creates a private room => CreatePrivateRoom(int userID, int opponentID); => ID of this room is sended to client;
        //Enable chat textBox on client
        //on send button we Invoke("SendToRoom", roomID);
        //here on server we save this messages in Room object

        public UserRepository UserRepository = new UserRepository();
        public RoomRepository RoomRepository = new RoomRepository();
        private ChatUser _tempUser { get; set; }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public void CreatePrivateRoom(string roomName)
        {
            var room = new Room(roomName, RoomType.Private);
            RoomRepository.AddRoom(room);
            RoomRepository.JoinRoom(_tempUser.ID, room.ID);
        }

        public void JoinGroup(int userID, int roomID)
        {
            RoomRepository.JoinRoom(userID, roomID);
        }

        public void SendMessagToGroup(int RoomId, string message)
        {
            var room = RoomRepository.FindById(RoomId);
            RoomRepository.WriteMessage(message, _tempUser, room);
        }

        public int GetUserId()
        {
            if (_tempUser != null)
                return _tempUser.ID;
            return 0;
        }

        public void Login(string username, string password)
        {
            var id = UserRepository.LoginUser(username, password);
            if (id != 0)
            {
                this._tempUser = UserRepository.FindById(id);
            }
            Clients.Caller.Login(id);
        }

        public void Register(string username, string password)
        {
            if (username != null || password != null)
            {
                this.UserRepository.AddUser(new ChatUser(username, password));
                Clients.Caller.Registered(true);
            }
            else
            {
                Clients.Caller.Registered(false);
            }
        }

        public void Send(string message)
        {
            Clients.All.send(message);
        }

        public List<RoomDTO> LoadUserRooms(int id)
        {
            var list = new List<RoomDTO>() { new RoomDTO() { RoomName = "Public Room", Type = RoomType.Public } };
            list.AddRange(UserRepository.FindAllUserPrivateContacts(id).ConvertAll(a => (RoomDTO)a));
            list.AddRange(RoomRepository.FindAllUserRooms(id).ConvertAll(a => (RoomDTO)a));
            return list;
        }
    }
}