using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenChat.Repositories;

namespace ChatTests
{
    [TestClass]
    public class RoomRepositoryTest
    {
        public const string TestName = "TestRoom";
        public RoomRepository RoomRepository { get; set; }

        [TestInitialize]
        public void Setup()
        {
            RoomRepository = new RoomRepository();
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
            var room = RoomRepository.FindAll().Count;
            RoomRepository.AddRoom(TestName);
            Assert.Equals(room++, RoomRepository.FindAll().Count);
        }

        [TestCleanup]
        public void RepositoryCleanup()
        {
            RoomRepository.DeleteRoom(TestName);
        }
    }
}