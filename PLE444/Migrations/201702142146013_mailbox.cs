namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mailbox : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PrivateMessages", "SenderId", c => c.String(maxLength: 128));
            AlterColumn("dbo.PrivateMessages", "ReceiverId", c => c.String(maxLength: 128));
            CreateIndex("dbo.PrivateMessages", "SenderId");
            CreateIndex("dbo.PrivateMessages", "ReceiverId");
            AddForeignKey("dbo.PrivateMessages", "ReceiverId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PrivateMessages", "SenderId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateMessages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PrivateMessages", "ReceiverId", "dbo.AspNetUsers");
            DropIndex("dbo.PrivateMessages", new[] { "ReceiverId" });
            DropIndex("dbo.PrivateMessages", new[] { "SenderId" });
            AlterColumn("dbo.PrivateMessages", "ReceiverId", c => c.String());
            AlterColumn("dbo.PrivateMessages", "SenderId", c => c.String());
        }
    }
}
