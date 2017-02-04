namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Documents", "OwnerId");
            AddForeignKey("dbo.Documents", "OwnerId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Documents", "Owner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "Owner", c => c.String());
            DropForeignKey("dbo.Documents", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Documents", new[] { "OwnerId" });
            DropColumn("dbo.Documents", "OwnerId");
        }
    }
}
