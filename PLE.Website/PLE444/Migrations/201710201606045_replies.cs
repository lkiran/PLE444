namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class replies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Message_ID", c => c.Guid());
            CreateIndex("dbo.Messages", "Message_ID");
            AddForeignKey("dbo.Messages", "Message_ID", "dbo.Messages", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Message_ID", "dbo.Messages");
            DropIndex("dbo.Messages", new[] { "Message_ID" });
            DropColumn("dbo.Messages", "Message_ID");
        }
    }
}
