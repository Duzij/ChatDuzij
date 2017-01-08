using Microsoft.AspNet.SignalR;
using OpenChat.Communication;
using OpenChat.Models;
using OpenChat.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OpenChat.Controllers
{
    public class RoomContoller : ApiController
    {
        public RoomRepository repo { get; set; }
        // GET api/<controller>
        public void PostJoinRoom(string nickname)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.login();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}