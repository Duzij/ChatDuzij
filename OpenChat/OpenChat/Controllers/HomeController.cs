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
        public RoomRepository repo = new RoomRepository();

        public ActionResult Index()
        {
            repo.WriteMessage("hello", "Admin", "Room123");
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}