using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatDuzijCore.Repositories;
using OpenChat.Models;
using OpenChat.Repositories;
using OpenChatClient.Model;

namespace OpenChat.Controllers
{
    public class HomeController : Controller
    {
        public UserRepository UserRepository = new UserRepository();
        public RoomRepository RoomRepository = new RoomRepository();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //var user = UserRepository.FindById(this.UserRepository.LoginUser("Admin", "123"));

            //var room = new Room("Room", RoomType.Group);
            //RoomRepository.AddRoom(room);
            //RoomRepository.JoinRoom(user.ID, room.ID);

            //var list = new List<RoomDTO>() { new RoomDTO() { RoomName = "Public Room", Type = RoomType.Public } };
            //list.AddRange(UserRepository.FindAllUserPrivateContacts(user.ID).ConvertAll(a => (RoomDTO)a));
            //list.AddRange(RoomRepository.FindAllUserRooms(user.ID).ConvertAll(a => (RoomDTO)a));

            return View();
        }
    }
}