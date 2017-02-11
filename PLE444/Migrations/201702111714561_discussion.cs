namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discussion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Discussions", "CreatorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Readings", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Discussions", "CreatorId");
            CreateIndex("dbo.Readings", "UserId");
            AddForeignKey("dbo.Discussions", "CreatorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Readings", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Readings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Discussions", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Readings", new[] { "UserId" });
            DropIndex("dbo.Discussions", new[] { "CreatorId" });
            AlterColumn("dbo.Readings", "UserId", c => c.String());
            AlterColumn("dbo.Discussions", "CreatorId", c => c.String());
        }
    }
}
