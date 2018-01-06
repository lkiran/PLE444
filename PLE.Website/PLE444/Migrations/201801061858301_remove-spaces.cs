namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removespaces : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "SpaceId", "dbo.Spaces");
            DropForeignKey("dbo.Communities", "SpaceId", "dbo.Spaces");
            DropIndex("dbo.Courses", new[] { "SpaceId" });
            DropIndex("dbo.Communities", new[] { "SpaceId" });
            DropColumn("dbo.Courses", "SpaceId");
            DropColumn("dbo.Communities", "SpaceId");
            DropTable("dbo.Spaces");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Spaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Communities", "SpaceId", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "SpaceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Communities", "SpaceId");
            CreateIndex("dbo.Courses", "SpaceId");
            AddForeignKey("dbo.Communities", "SpaceId", "dbo.Spaces", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Courses", "SpaceId", "dbo.Spaces", "Id", cascadeDelete: true);
        }
    }
}
