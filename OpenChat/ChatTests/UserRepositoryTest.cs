using System;
using ChatDuzijCore.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenChat.Communication;
using OpenChat.Models;
using OpenChat.Repositories;
using OpenChatClient.Models;

namespace ChatTests
{
    [TestClass]
    public class UserRepositoryTest
    {
        public RoomRepository RoomRepository { get; set; }
        public UserRepository UserRepository { get; set; }

        public Message msg { get; set; } = new Message() { Author = "TestUser", Room = "TestRoom", Text = "TestMessage" };
        public User user { get; set; } = new User() { Username = "TestUser", Password = "testPassword" };

        public Room room { get; set; } = new Room() { RoomName = "TestRoom" };

        [TestInitialize]
        public void Setup()
        {
            UserRepository = new UserRepository();
        }

        [TestMethod]
        public void GetAllUsers()
        {
            UserRepository.AddUser(user);
            var users = UserRepository.FindAll();
            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void AddRoom()
        {
            var users = UserRepository.FindAll().Count;
            UserRepository.AddUser(user);
            Assert.AreEqual(users + 1, UserRepository.FindAll().Count);
        }

        [TestMethod]
        public void LoginUser()
        {
            UserRepository.AddUser(user);
            Assert.AreSame(user.Username, UserRepository.LoginUser(user.Username, user.Password));
        }

        [TestMethod]
        public void BadLoginUser()
        {
            UserRepository.AddUser(user);
            Assert.AreSame("404", UserRepository.LoginUser(user.Username + "bad", user.Password));
        }

        [TestMethod]
        public void AddIdentity()
        {
            UserRepository.AddUser(user);
            UserRepository.AddIdentity(user.Username, "5cac6258-de7c-436f-abc8-5262cc53eb3e");
            Assert.IsTrue(UserRepository.IsUserConnected(user.Username));
            UserRepository.RemoveIndentity("5cac6258-de7c-436f-abc8-5262cc53eb3e");
        }

        [TestCleanup]
        public void RepositoryCleanup()
        {
            UserRepository.DeleteUser(user);
        }
    }
}