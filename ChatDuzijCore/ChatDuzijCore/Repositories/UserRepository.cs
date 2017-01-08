using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatDuzijCore.Models;
using ChatDuzijCore.Models.ChatModel;
using Swashbuckle.Swagger;

namespace ChatDuzijCore.Repositories
{
    public class UserRepository
    {
        public ApplicationDbContext Context { get; set; }

        public ChatUser FindById(int id)
        {
            return Context.Users.Find(id);
        }

        public IEnumerable<ChatUser> FindAll()
        {
            return Context.Users;
        }

        public void AddUser(ChatUser u)
        {
            this.Context.Users.Add(u);
            this.Context.SaveChanges();
        }

        public void DeleteUser(ChatUser u)
        {
            this.Context.Users.Remove(u);
            this.Context.SaveChanges();
        }

        public void EditUser(ChatUser u)
        {
            var user = this.FindById(u.ID);
            this.Context.Users.Remove(user);
            this.Context.Users.Add(u);
            this.Context.SaveChanges();
        }

        public IEnumerable<ChatUser> GetUserContacts(int userId)
        {
            return FindById(userId).Contacts;
        }

        public bool LoginUser(string username, string password)
        {
            var users = this.FindAll().ToArray();
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].Username == username)
                {
                    if (users[i].Password == password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}