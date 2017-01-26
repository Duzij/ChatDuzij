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

        public void AddIdentity(string username, string connectionID)
        {
            if (!this.IsUserConnected(username))
                this.Context.ConnectedUsers.Add(new UserIdentity() { Username = username, ConnectionID = connectionID });
            this.Context.SaveChanges();
        }

        public void RemoveIndentity(string connection)
        {
            var identity = this.GetIdentityByConnection(connection);
            this.Context.ConnectedUsers.Remove(identity);
            this.Context.SaveChanges();
        }

        public string GetUserConnectionByUsername(string username)
        {
            return Context.ConnectedUsers.First(a => a.Username == username).ConnectionID;
        }

        public bool IsUserConnected(string username)
        {
            return Context.ConnectedUsers.Any(a => a.Username == username);
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

        private UserIdentity GetIdentityByUsername(string username)
        {
            return Context.ConnectedUsers.First(a => a.Username == username);
        }

        private UserIdentity GetIdentityByConnection(string connection)
        {
            return Context.ConnectedUsers.First(a => a.ConnectionID == connection);
        }
    }
}