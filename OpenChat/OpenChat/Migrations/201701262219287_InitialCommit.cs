namespace OpenChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCommit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserIdentity",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ConnectionID = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.Room",
                c => new
                    {
                        RoomName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.RoomName);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(unicode: false),
                        Timestamp = c.Double(nullable: false),
                        Author = c.String(unicode: false),
                        Room = c.String(unicode: false),
                        Room_RoomName = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Room", t => t.Room_RoomName)
                .Index(t => t.Room_RoomName);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Password = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.UserRoom",
                c => new
                    {
                        User_Username = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Room_RoomName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.User_Username, t.Room_RoomName })
                .ForeignKey("dbo.User", t => t.User_Username, cascadeDelete: true)
                .ForeignKey("dbo.Room", t => t.Room_RoomName, cascadeDelete: true)
                .Index(t => t.User_Username)
                .Index(t => t.Room_RoomName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoom", "Room_RoomName", "dbo.Room");
            DropForeignKey("dbo.UserRoom", "User_Username", "dbo.User");
            DropForeignKey("dbo.Message", "Room_RoomName", "dbo.Room");
            DropIndex("dbo.UserRoom", new[] { "Room_RoomName" });
            DropIndex("dbo.UserRoom", new[] { "User_Username" });
            DropIndex("dbo.Message", new[] { "Room_RoomName" });
            DropTable("dbo.UserRoom");
            DropTable("dbo.User");
            DropTable("dbo.Message");
            DropTable("dbo.Room");
            DropTable("dbo.UserIdentity");
        }
    }
}
