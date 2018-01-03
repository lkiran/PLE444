namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "IsFeedbackPublished", c => c.Boolean(nullable: false));
            AddColumn("dbo.Courses", "IsCourseActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Documents", "Feedback", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Feedback");
            DropColumn("dbo.Courses", "IsCourseActive");
            DropColumn("dbo.Assignments", "IsFeedbackPublished");
        }
    }
}
