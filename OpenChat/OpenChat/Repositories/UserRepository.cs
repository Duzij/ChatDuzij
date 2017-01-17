using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenChat.Models;

namespace ChatDuzijCore.Repositories
{
    public class UserRepository
    {
        public ChatDbContext Context = new ChatDbContext();

        public Room Find(string name)
        {
            return Context.Rooms.Find(name);
        }

        public List<User> FindAll()
        {
            return Context.Users.ToList();
        }

        public void AddUser(User u)
        {
            this.Context.Users.Add(u);
            this.Context.SaveChanges();
        }

        public void DeleteUser(User u)
        {
            this.Context.Users.Remove(u);
            this.Context.SaveChanges();
        }

        public string LoginUser(string username, string password)
        {
            var users = this.FindAll().ToArray();
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].Username == username)
                {
                    if (users[i].Password == password)
                    {
                        return username;
                    }
                }
            }
            return "404";
        }
    }
}