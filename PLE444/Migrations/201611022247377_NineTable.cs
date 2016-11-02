namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NineTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        FilePath = c.String(nullable: false, maxLength: 128),
                        Owner = c.String(),
                        Description = c.String(),
                        Rate = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.FilePath);
            
            CreateTable(
                "dbo.EventReponses",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Status = c.Int(nullable: false),
                        Event_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Events", t => t.Event_ID)
                .Index(t => t.Event_ID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Place = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.UserCommunities",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApprovalDate = c.DateTime(nullable: false),
                        Community_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Communities", t => t.Community_ID)
                .Index(t => t.Community_ID);
            
            CreateTable(
                "dbo.UserCourses",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApprovalDate = c.DateTime(nullable: false),
                        Course_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .Index(t => t.Course_ID);
            
            CreateTable(
                "dbo.UserInterests",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Course_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .Index(t => t.Course_ID);
            
            AddColumn("dbo.UserDetails", "About", c => c.String());
            AlterColumn("dbo.UserDetails", "Name", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserDetails", "Name");
            AddForeignKey("dbo.UserDetails", "Name", "dbo.Roles", "Name");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInterests", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.UserDetails", "Name", "dbo.Roles");
            DropForeignKey("dbo.UserCourses", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.UserCommunities", "Community_ID", "dbo.Communities");
            DropForeignKey("dbo.EventReponses", "Event_ID", "dbo.Events");
            DropIndex("dbo.UserInterests", new[] { "Course_ID" });
            DropIndex("dbo.UserDetails", new[] { "Name" });
            DropIndex("dbo.UserCourses", new[] { "Course_ID" });
            DropIndex("dbo.UserCommunities", new[] { "Community_ID" });
            DropIndex("dbo.EventReponses", new[] { "Event_ID" });
            AlterColumn("dbo.UserDetails", "Name", c => c.String());
            DropColumn("dbo.UserDetails", "About");
            DropTable("dbo.UserInterests");
            DropTable("dbo.UserCourses");
            DropTable("dbo.UserCommunities");
            DropTable("dbo.Roles");
            DropTable("dbo.Interests");
            DropTable("dbo.Events");
            DropTable("dbo.EventReponses");
            DropTable("dbo.Documents");
            DropTable("dbo.Courses");
        }
    }
}
