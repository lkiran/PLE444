namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class course_approval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCourses", "DateJoin", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserCourses", "DateJoin");
        }
    }
}
