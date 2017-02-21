namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class community : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserCommunities", "Community_ID", "dbo.Communities");
            DropIndex("dbo.Discussions", new[] { "Community_ID" });
            DropIndex("dbo.UserCommunities", new[] { "Community_ID" });
            RenameColumn(table: "dbo.UserCommunities", name: "Community_ID", newName: "CommunityId");
            AddColumn("dbo.Communities", "OwnerId", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserCommunities", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserCommunities", "DateJoined", c => c.DateTime());
            AlterColumn("dbo.UserCommunities", "CommunityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Discussions", "Community_Id");
            CreateIndex("dbo.Communities", "OwnerId");
            CreateIndex("dbo.Communities", "SpaceId");
            CreateIndex("dbo.UserCommunities", "UserId");
            CreateIndex("dbo.UserCommunities", "CommunityId");
            AddForeignKey("dbo.Communities", "OwnerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Communities", "SpaceId", "dbo.Spaces", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserCommunities", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserCommunities", "CommunityId", "dbo.Communities", "Id", cascadeDelete: true);
            DropColumn("dbo.Communities", "canCreateEvents");
            DropColumn("dbo.Communities", "GroupPhoto");
            DropColumn("dbo.Communities", "AdminId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Communities", "AdminId", c => c.String());
            AddColumn("dbo.Communities", "GroupPhoto", c => c.String());
            AddColumn("dbo.Communities", "canCreateEvents", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.UserCommunities", "CommunityId", "dbo.Communities");
            DropForeignKey("dbo.UserCommunities", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Communities", "SpaceId", "dbo.Spaces");
            DropForeignKey("dbo.Communities", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.UserCommunities", new[] { "CommunityId" });
            DropIndex("dbo.UserCommunities", new[] { "UserId" });
            DropIndex("dbo.Communities", new[] { "SpaceId" });
            DropIndex("dbo.Communities", new[] { "OwnerId" });
            DropIndex("dbo.Discussions", new[] { "Community_Id" });
            AlterColumn("dbo.UserCommunities", "CommunityId", c => c.Guid());
            AlterColumn("dbo.UserCommunities", "DateJoined", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserCommunities", "UserId", c => c.String());
            DropColumn("dbo.Communities", "OwnerId");
            RenameColumn(table: "dbo.UserCommunities", name: "CommunityId", newName: "Community_ID");
            CreateIndex("dbo.UserCommunities", "Community_ID");
            CreateIndex("dbo.Discussions", "Community_ID");
            AddForeignKey("dbo.UserCommunities", "Community_ID", "dbo.Communities", "ID");
        }
    }
}
