namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recreate : DbMigration
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        Code = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        CanEveryoneJoin = c.Boolean(nullable: false),
                        SpaceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.Spaces", t => t.SpaceId, cascadeDelete: true)
                .Index(t => t.CreatorId)
                .Index(t => t.SpaceId);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CourseId = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                     //   OrderBy = c.String(nullable: true),
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
                        CreatorId = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Course_Id = c.Guid(),
                        Community_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Communities", t => t.Community_ID)
                .Index(t => t.Course_Id)
                .Index(t => t.Community_ID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Content = c.String(),
                        SenderId = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        Discussion_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Discussions", t => t.Discussion_ID)
                .Index(t => t.Discussion_ID);
            
            CreateTable(
                "dbo.Readings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Date = c.DateTime(nullable: false),
                        Discussion_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Discussions", t => t.Discussion_ID)
                .Index(t => t.Discussion_ID);
            
            CreateTable(
                "dbo.Spaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        isOpen = c.Boolean(nullable: false),
                        isHiden = c.Boolean(nullable: false),
                        canCreateEvents = c.Boolean(nullable: false),
                        GroupPhoto = c.String(),
                        AdminId = c.String(),
                        SpaceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                        SenderId = c.String(),
                        ReceiverId = c.String(),
                        Content = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        isRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        UserId = c.String(),
                        DateJoined = c.DateTime(nullable: false),
                        Community_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Communities", t => t.Community_ID)
                .Index(t => t.Community_ID);
            
            CreateTable(
                "dbo.UserCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
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
            DropForeignKey("dbo.UserCommunities", "Community_ID", "dbo.Communities");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GradeTypes", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Discussions", "Community_ID", "dbo.Communities");
            DropForeignKey("dbo.Documents", "Assignment_Id", "dbo.Assignments");
            DropForeignKey("dbo.Assignments", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.TimelineEntries", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.TimelineEntries", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "SpaceId", "dbo.Spaces");
            DropForeignKey("dbo.Discussions", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Readings", "Discussion_ID", "dbo.Discussions");
            DropForeignKey("dbo.Messages", "Discussion_ID", "dbo.Discussions");
            DropForeignKey("dbo.Courses", "CreatorId", "dbo.AspNetUsers");
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
            DropIndex("dbo.UserCommunities", new[] { "Community_ID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.GradeTypes", new[] { "CourseId" });
            DropIndex("dbo.TimelineEntries", new[] { "Course_Id" });
            DropIndex("dbo.TimelineEntries", new[] { "CreatorId" });
            DropIndex("dbo.Readings", new[] { "Discussion_ID" });
            DropIndex("dbo.Messages", new[] { "Discussion_ID" });
            DropIndex("dbo.Discussions", new[] { "Community_ID" });
            DropIndex("dbo.Discussions", new[] { "Course_Id" });
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
            DropIndex("dbo.Courses", new[] { "SpaceId" });
            DropIndex("dbo.Courses", new[] { "CreatorId" });
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
            DropTable("dbo.Spaces");
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
