using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatDuzijCore.Repositories;
using OpenChat.Models;

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
            this.Context.Rooms.Remove(room);
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
            var room = Context.Rooms.Where(a => a.RoomName == roomName).First();
            room.Messages.Add(msg);
            this.Context.SaveChanges();
        }

        public void JoinRoom(string username, string roomName)
        {
            var user = Context.Users.Find(username);
            Context.Rooms.Find(roomName).Users.Add(user);
            this.Context.SaveChanges();
        }

        public List<Room> FindAllUserRooms(string name)
        {
            return Context.Users.Find(name).Rooms;
        }
    }
}