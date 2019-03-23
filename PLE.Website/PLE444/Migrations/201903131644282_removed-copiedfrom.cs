namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedcopiedfrom : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "CopiedFromId", "dbo.Courses");
            DropIndex("dbo.Courses", new[] { "CopiedFromId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Courses", "CopiedFromId");
            AddForeignKey("dbo.Courses", "CopiedFromId", "dbo.Courses", "Id");
        }
    }
}
