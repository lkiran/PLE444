namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class optionalcopiedfrom : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Courses", new[] { "CopiedFromId" });
            AlterColumn("dbo.Courses", "CopiedFromId", c => c.Guid());
            CreateIndex("dbo.Courses", "CopiedFromId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Courses", new[] { "CopiedFromId" });
            AlterColumn("dbo.Courses", "CopiedFromId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Courses", "CopiedFromId");
        }
    }
}
