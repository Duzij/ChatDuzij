using System;
using ChatDuzijCore.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenChat.Communication;
using OpenChat.Models;
using OpenChat.Repositories;
using OpenChatClient.Model;

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

        [TestMethod]
        public void WriteMessage()
        {
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
    }
}