using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace OpenChat.Models
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ChatDbContext : DbContext
    {
        public ChatDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ChatDbContext>());

            var testadmin = new ChatUser() { ID = 0, Username = "Admin" };
            var testUser = new ChatUser() { ID = 1, Username = "AdminTest" };
            Users.Add(testUser);
            Users.Add(testadmin);

            var room = new Room()
            {
                ID = 0,
                Messages = new List<Message>() { },
                Users = new List<ChatUser>() { testadmin, testUser },
                RoomName = "Room1"
            };

            var testMsg = new Message() { ID = 0, Author = testUser, AuthorID = 1, Text = "This text is a test", Room = room, RoomID = 0 };
            room.Messages.Add(testMsg);

            Rooms.Add(room);

            this.SaveChanges();
        }

        public DbSet<ChatUser> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}