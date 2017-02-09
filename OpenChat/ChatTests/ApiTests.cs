using System;
using System.Linq;
using ChatDuzijCore.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenChat.Communication;
using OpenChat.Models;
using OpenChat.Repositories;
using OpenChatClient.Models;

namespace ChatTests
{
    [TestClass]
    public class ApiTests
    {
        public ChatHub ChatHub { get; set; }

        [TestInitialize]
        public void Setup()
        {
            ChatHub = new ChatHub();
        }

        /// <summary>
        /// Testing if message is written
        /// </summary>
        [TestMethod]
        public void WriteMessage()
        {
            ChatHub.RoomRepository.AddRoom("TestRoom");
            var messagesBefore = ChatHub.RoomRepository.GetAllMessages("TestRoom").Count;
            ChatHub.UserRepository.AddUser(new User() { Username = "TestUser", Password = "TestPasword" });
            ChatHub.RoomRepository.AddRoom("TestName");

            ChatHub.SendMessage("TestRoom", "TestMessage", "TestUser");
            Assert.Equals(ChatHub.RoomRepository.GetAllMessages("TestRoom").Count, messagesBefore++);
        }

        /// <summary>
        /// Unknown user is added anyway
        /// </summary>
        [TestMethod]
        public void LoginUnknownUser()
        {
            var countBefore = ChatHub.UserRepository.FindAll().Count;
            UserDTO user = new UserDTO() { Username = "TestUser" };
            ChatHub.Login(user.Username, "testPassword");
            Assert.AreEqual(countBefore++, ChatHub.UserRepository.FindAll().Count);
        }

        [TestCleanup]
        public void RepositoryCleanup()
        {
            ChatHub.RoomRepository.DeleteRoom("TestUser");
            ChatHub.RoomRepository.DeleteRoom("TestRoom");
        }
    }
}