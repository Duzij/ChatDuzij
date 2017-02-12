using System;
using ChatDuzijCore.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenChat.Repositories;
using OpenChatClient.Models;

namespace ChatTests
{
    [TestClass]
    public class RoomRepositoryTest
    {
        public RoomRepository RoomRepository { get; set; }
        public UserRepository UserRepository { get; set; }

        public Message msg { get; set; } = new Message() { Author = "TestUser", Room = "TestRoom", Text = "TestMessage" };
        public User user { get; set; } = new User() { Username = "TestUser", Password = "testPassword" };

        public Room room { get; set; } = new Room() { RoomName = "TestRoom" };

        [TestInitialize]
        public void Setup()
        {
            RoomRepository = new RoomRepository();
            UserRepository = new UserRepository();
            UserRepository.AddUser(user);
            RoomRepository.AddRoom(room.RoomName);
        }

        [TestMethod]
        public void GetAllRooms()
        {
            var rooms = RoomRepository.FindAll();
            Assert.IsTrue(rooms.Count > 0);
        }

        [TestMethod]
        public void AddRoom()
        {
            var roomCount = RoomRepository.FindAll().Count;
            RoomRepository.AddRoom(room.RoomName + "2");
            Assert.AreEqual(roomCount + 1, RoomRepository.FindAll().Count);
            RoomRepository.DeleteRoom(room.RoomName + "2");
        }

        [TestMethod]
        public void AddUserToRoom()
        {
            var usersCount = RoomRepository.Find(room.RoomName).Users.Count;
            RoomRepository.JoinRoom(user.Username, room.RoomName);
            Assert.AreEqual(usersCount + 1, RoomRepository.Find(room.RoomName).Users.Count);
        }

        [TestCleanup]
        public void RepositoryCleanup()
        {
            RoomRepository.DeleteRoom(room.RoomName);
            UserRepository.DeleteUser(user);
        }
    }
}