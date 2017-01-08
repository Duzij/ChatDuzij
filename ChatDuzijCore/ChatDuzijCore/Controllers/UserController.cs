using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ChatDuzijCore.Models.ChatModel;
using ChatDuzijCore.Repositories;

namespace ChatDuzijCore.Controllers
{
    public class UserController
    {

        public UserRepository rep { get; set; }

        public ChatUser GetUserById(int id)
        {
            return rep.FindById(id);
        }


        // POST api/values
        public bool GetLoginUser([FromBody]string username, [FromBody] string password)
        {
            if (rep.LoginUser(username, password))
                return true;
            else
                return false;
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