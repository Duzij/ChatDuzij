using System;
using ChatDuzijCore.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenChat.Communication;
using OpenChat.Repositories;

namespace ChatTests
{
    [TestClass]
    public class ApiTests
    {
        public RoomRepository RoomRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public ChatHub ChatHub { get; set; }

        [TestInitialize]
        public void Setup()
        {
            ChatHub = new ChatHub();
        }

        [TestMethod]
        public void AddingRoom()
        {
            RoomRepository roomRepository = new RoomRepository();
            roomRepository.AddRoom("TestRoom");
        }
    }
}