namespace OpenChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Room",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoomName = c.String(unicode: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(unicode: false),
                        Timestamp = c.Double(nullable: false),
                        AuthorID = c.Int(nullable: false),
                        RoomID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ChatUser", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.Room", t => t.RoomID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.RoomID);
            
            CreateTable(
                "dbo.ChatUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(unicode: false),
                        Password = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ChatUserRoom",
                c => new
                    {
                        ChatUser_ID = c.Int(nullable: false),
                        Room_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChatUser_ID, t.Room_ID })
                .ForeignKey("dbo.ChatUser", t => t.ChatUser_ID, cascadeDelete: true)
                .ForeignKey("dbo.Room", t => t.Room_ID, cascadeDelete: true)
                .Index(t => t.ChatUser_ID)
                .Index(t => t.Room_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "RoomID", "dbo.Room");
            DropForeignKey("dbo.Message", "AuthorID", "dbo.ChatUser");
            DropForeignKey("dbo.ChatUserRoom", "Room_ID", "dbo.Room");
            DropForeignKey("dbo.ChatUserRoom", "ChatUser_ID", "dbo.ChatUser");
            DropIndex("dbo.ChatUserRoom", new[] { "Room_ID" });
            DropIndex("dbo.ChatUserRoom", new[] { "ChatUser_ID" });
            DropIndex("dbo.Message", new[] { "RoomID" });
            DropIndex("dbo.Message", new[] { "AuthorID" });
            DropTable("dbo.ChatUserRoom");
            DropTable("dbo.ChatUser");
            DropTable("dbo.Message");
            DropTable("dbo.Room");
        }
    }
}
