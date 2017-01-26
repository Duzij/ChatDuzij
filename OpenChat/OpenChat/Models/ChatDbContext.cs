using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Devtalk.EF.CodeFirst;

namespace OpenChat.Models
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ChatDbContext : DbContext
    {
        public ChatDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DbContext>());
        }

        public DbSet<UserIdentity> ConnectedUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}