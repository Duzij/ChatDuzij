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
        public UserRepository repo { get; set; }
        public RoomRepository roomRepository = new RoomRepository();
        public ActionResult Index()
        {
            //roomRepository.AddRoom("Popokoj");
            //List<string> users = new List<string>() { "Admin", "User"};
            //foreach (var item in users)
            //{
            //    roomRepository.JoinRoom(item, "Popokoj");
            //}

            ViewBag.Title = "Home Page";
            return View();
        }
    }
}