namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Message_ID", c => c.Guid());
            CreateIndex("dbo.Messages", "Message_ID");
            AddForeignKey("dbo.Messages", "Message_ID", "dbo.Messages", "ID");
            DropColumn("dbo.Messages", "ResponseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "ResponseId", c => c.Int());
            DropForeignKey("dbo.Messages", "Message_ID", "dbo.Messages");
            DropIndex("dbo.Messages", new[] { "Message_ID" });
            DropColumn("dbo.Messages", "Message_ID");
        }
    }
}
