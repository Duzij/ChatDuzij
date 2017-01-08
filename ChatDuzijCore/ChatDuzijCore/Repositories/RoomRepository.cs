using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatDuzijCore.Models;
using ChatDuzijCore.Models.ChatModel;

namespace ChatDuzijCore.Repositories
{
    public class RoomRepository
    {
        private UserRepository userRepository;

        public RoomRepository(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public ApplicationDbContext Context { get; set; }

        public Room FindById(int id)
        {
            return Context.Rooms.Find(id);
        }

        public IEnumerable<Room> FindAll()
        {
            return Context.Rooms;
        }

        public void AddUser(Room u)
        {
            this.Context.Rooms.Add(u);
            this.Context.SaveChanges();
        }

        public void DeleteUser(Room u)
        {
            this.Context.Rooms.Remove(u);
            this.Context.SaveChanges();
        }

        public void EditUser(Room u)
        {
            var user = this.FindById(u.ID);
            this.Context.Rooms.Remove(user);
            this.Context.Rooms.Add(u);
            this.Context.SaveChanges();
        }

        public List<Message> GetAllMessagesById(int roomId)
        {
            var currentRoom = this.FindById(roomId);
            return Context.Rooms.Find(currentRoom).Messages;
        }

        public void WriteMessage(string message, ChatUser author, Room room)
        {
            var msg = new Message() { Author = author, Room = room, Text = message };
            var currentRoom = this.FindById(room.ID);
            Context.Rooms.Find(currentRoom).Messages.Add(msg);
            this.Context.SaveChanges();
        }

        public IEnumerable<Room> FindAllUserRooms(int userId)
        {
            return userRepository.FindById(userId).Rooms;
        }
    }
}