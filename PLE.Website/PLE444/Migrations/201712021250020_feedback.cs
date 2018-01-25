namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class feedback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "IsFeedbackPublished", c => c.Boolean(nullable: false));
            DropColumn("dbo.Documents", "IsFeedbackPublished");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "IsFeedbackPublished", c => c.Boolean(nullable: false));
            DropColumn("dbo.Assignments", "IsFeedbackPublished");
        }
    }
}
