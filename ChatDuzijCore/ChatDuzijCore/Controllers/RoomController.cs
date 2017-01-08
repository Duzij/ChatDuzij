using ChatDuzijCore.Models.ChatModel;
using ChatDuzijCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ChatDuzijCore.Controllers
{
    public class RoomController : ApiController
    {
        public RoomRepository rep { get; set; }

        public Room GetUserById(int id)
        {
            return rep.FindById(id);
        }

        // POST api/values
        public List<Message> GetRoomMessages(int roomId)
        {
            return (rep.GetAllMessagesById(roomId));
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

    }
}