namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activeCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "IsCourseActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "IsCourseActive");
        }
    }
}
