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
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}