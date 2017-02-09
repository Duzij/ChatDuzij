using System;
using ChatDuzijCore.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenChat.Communication;
using OpenChat.Models;

namespace ChatTests
{
    [TestClass]
    public class UserRepositoryTest
    {
        public const string TestName = "TestUser";

        public const string TestPassword = "TestPassword";
        public UserRepository UserRepository { get; set; }

        [TestInitialize]
        public void Setup()
        {
            UserRepository = new UserRepository();
        }

        [TestMethod]
        public void GetAllUsers()
        {
            var rooms = UserRepository.FindAll();
            Assert.IsTrue(rooms.Count > 0);
        }

        [TestMethod]
        public void AddRoom()
        {
            var room = UserRepository.FindAll().Count;
            UserRepository.AddUser(new User() { Username = TestName, Password = TestPassword });
            Assert.Equals(room++, UserRepository.FindAll().Count);
        }

        [TestCleanup]
        public void RepositoryCleanup()
        {
            UserRepository.DeleteUser(new User(TestName, TestPassword));
        }
    }
}