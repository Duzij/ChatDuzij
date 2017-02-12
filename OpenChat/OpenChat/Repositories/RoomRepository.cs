using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ChatDuzijCore.Repositories;
using OpenChat.Models;
using OpenChatClient.Models;

namespace OpenChat.Repositories
{
    public class RoomRepository
    {
        public ChatDbContext Context = new ChatDbContext();

        public Room Find(string name)
        {
            return Context.Rooms.Find(name);
        }

        public List<Room> FindAll()
        {
            return Context.Rooms.ToList();
        }

        public void AddRoom(string roomName)
        {
            this.Context.Rooms.Add(new Room(roomName));
            this.Context.SaveChanges();
        }

        public void DeleteRoom(string name)
        {
            var room = this.Find(name);
            if (room != null)
            {
                this.Context.Rooms.Remove(room);
            }

            this.Context.SaveChanges();
        }

        public List<Message> GetAllMessages(string name)
        {
            if (Context.Rooms.Find(name).Messages == null)
                return new List<Message>();
            else
                return Context.Rooms.Find(name).Messages;
        }

        public void WriteMessage(string message, string authorName, string roomName)
        {
            var msg = new Message() { Author = authorName, Room = authorName, Text = message };
            var room = Context.Rooms.First(a => a.RoomName == roomName);
            room.Messages.Add(msg);
            this.Context.SaveChanges();
        }

        public void DeleteMessage(Message msg, string roomName)
        {
            var room = Context.Rooms.First(a => a.RoomName == roomName);
            room.Messages.Remove(msg);
            this.Context.SaveChanges();
        }

        public void JoinRoom(string username, string roomName)
        {
            var room = this.Find(roomName);
            Context.Users.Find(username).Rooms.Add(room);
            this.Context.SaveChanges();
        }

        public List<Room> FindAllUserRooms(string name)
        {
            return Context.Users.Find(name).Rooms;
        }
    }
}