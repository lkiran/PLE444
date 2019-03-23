namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class courseterm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CourseId = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        IsFeedbackPublished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CopiedFromId = c.Guid(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        Code = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        CanEveryoneJoin = c.Boolean(nullable: false),
                        IsCourseActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CopiedFromId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .Index(t => t.CopiedFromId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CourseId = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        OrderBy = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        Chapter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .ForeignKey("dbo.Chapters", t => t.Chapter_Id)
                .Index(t => t.OwnerId)
                .Index(t => t.Chapter_Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.String(maxLength: 128),
                        FilePath = c.String(),
                        Description = c.String(),
                        DateUpload = c.DateTime(nullable: false),
                        Feedback = c.String(),
                        Material_Id = c.Guid(),
                        Assignment_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .ForeignKey("dbo.Materials", t => t.Material_Id)
                .ForeignKey("dbo.Assignments", t => t.Assignment_Id)
                .Index(t => t.OwnerId)
                .Index(t => t.Material_Id)
                .Index(t => t.Assignment_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        ProfilePicture = c.String(),
                        PhoneNo = c.String(),
                        Vision = c.String(),
                        Mission = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Discussions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Topic = c.String(),
                        CreatorId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        Course_Id = c.Guid(),
                        Community_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Communities", t => t.Community_Id)
                .Index(t => t.CreatorId)
                .Index(t => t.Course_Id)
                .Index(t => t.Community_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Content = c.String(),
                        SenderId = c.String(maxLength: 128),
                        DateSent = c.DateTime(nullable: false),
                        Message_ID = c.Guid(),
                        Discussion_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Messages", t => t.Message_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .ForeignKey("dbo.Discussions", t => t.Discussion_ID)
                .Index(t => t.SenderId)
                .Index(t => t.Message_ID)
                .Index(t => t.Discussion_ID);
            
            CreateTable(
                "dbo.Readings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Discussion_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Discussions", t => t.Discussion_ID)
                .Index(t => t.UserId)
                .Index(t => t.Discussion_ID);
            
            CreateTable(
                "dbo.TimelineEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        Heading = c.String(),
                        Content = c.String(),
                        ColorClass = c.String(),
                        IconClass = c.String(),
                        Course_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.CreatorId)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.Communities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsOpen = c.Boolean(nullable: false),
                        IsHiden = c.Boolean(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        userID = c.String(),
                        FriendID = c.String(),
                        isApproved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GradeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        CourseId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Effect = c.Int(nullable: false),
                        MaxScore = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.PrivateMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.String(maxLength: 128),
                        ReceiverId = c.String(maxLength: 128),
                        Content = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserCommunities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CommunityId = c.Guid(nullable: false),
                        DateJoined = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Communities", t => t.CommunityId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CommunityId);
            
            CreateTable(
                "dbo.UserCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        DateJoin = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                        CourseId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.UserGrades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        GradeTypeId = c.Int(nullable: false),
                        Grade = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GradeTypes", t => t.GradeTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.GradeTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGrades", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGrades", "GradeTypeId", "dbo.GradeTypes");
            DropForeignKey("dbo.UserCourses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.UserCommunities", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCommunities", "CommunityId", "dbo.Communities");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PrivateMessages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PrivateMessages", "ReceiverId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GradeTypes", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Communities", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Discussions", "Community_Id", "dbo.Communities");
            DropForeignKey("dbo.Documents", "Assignment_Id", "dbo.Assignments");
            DropForeignKey("dbo.Assignments", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.TimelineEntries", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.TimelineEntries", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Discussions", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Readings", "Discussion_ID", "dbo.Discussions");
            DropForeignKey("dbo.Readings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Discussion_ID", "dbo.Discussions");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Message_ID", "dbo.Messages");
            DropForeignKey("dbo.Discussions", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "CopiedFromId", "dbo.Courses");
            DropForeignKey("dbo.Materials", "Chapter_Id", "dbo.Chapters");
            DropForeignKey("dbo.Materials", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "Material_Id", "dbo.Materials");
            DropForeignKey("dbo.Documents", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chapters", "CourseId", "dbo.Courses");
            DropIndex("dbo.UserGrades", new[] { "GradeTypeId" });
            DropIndex("dbo.UserGrades", new[] { "UserId" });
            DropIndex("dbo.UserCourses", new[] { "CourseId" });
            DropIndex("dbo.UserCourses", new[] { "UserId" });
            DropIndex("dbo.UserCommunities", new[] { "CommunityId" });
            DropIndex("dbo.UserCommunities", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PrivateMessages", new[] { "ReceiverId" });
            DropIndex("dbo.PrivateMessages", new[] { "SenderId" });
            DropIndex("dbo.GradeTypes", new[] { "CourseId" });
            DropIndex("dbo.Communities", new[] { "OwnerId" });
            DropIndex("dbo.TimelineEntries", new[] { "Course_Id" });
            DropIndex("dbo.TimelineEntries", new[] { "CreatorId" });
            DropIndex("dbo.Readings", new[] { "Discussion_ID" });
            DropIndex("dbo.Readings", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "Discussion_ID" });
            DropIndex("dbo.Messages", new[] { "Message_ID" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Discussions", new[] { "Community_Id" });
            DropIndex("dbo.Discussions", new[] { "Course_Id" });
            DropIndex("dbo.Discussions", new[] { "CreatorId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Documents", new[] { "Assignment_Id" });
            DropIndex("dbo.Documents", new[] { "Material_Id" });
            DropIndex("dbo.Documents", new[] { "OwnerId" });
            DropIndex("dbo.Materials", new[] { "Chapter_Id" });
            DropIndex("dbo.Materials", new[] { "OwnerId" });
            DropIndex("dbo.Chapters", new[] { "CourseId" });
            DropIndex("dbo.Courses", new[] { "CreatorId" });
            DropIndex("dbo.Courses", new[] { "CopiedFromId" });
            DropIndex("dbo.Assignments", new[] { "CourseId" });
            DropTable("dbo.UserGrades");
            DropTable("dbo.UserCourses");
            DropTable("dbo.UserCommunities");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PrivateMessages");
            DropTable("dbo.GradeTypes");
            DropTable("dbo.Friendships");
            DropTable("dbo.Communities");
            DropTable("dbo.TimelineEntries");
            DropTable("dbo.Readings");
            DropTable("dbo.Messages");
            DropTable("dbo.Discussions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Documents");
            DropTable("dbo.Materials");
            DropTable("dbo.Chapters");
            DropTable("dbo.Courses");
            DropTable("dbo.Assignments");
        }
    }
}
