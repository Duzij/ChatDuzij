using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatDuzijCore.Repositories;
using OpenChat.Communication;
using OpenChat.Models;
using OpenChat.Repositories;

namespace OpenChat.Controllers
{
    public class HomeController : Controller
    {
        public ChatHub ChatHub = new ChatHub();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            ChatHub.Login("Admin", "123");

            //var room = new Room("Room", RoomType.Group);
            //RoomRepository.AddRoom(room);
            //RoomRepository.JoinRoom(user.ID, room.ID);

            return View();
        }
    }
}