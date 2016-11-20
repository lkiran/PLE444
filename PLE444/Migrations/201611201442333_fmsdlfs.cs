namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fmsdlfs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communities", "isOpen", c => c.Boolean(nullable: false));
            AddColumn("dbo.Communities", "isHiden", c => c.Boolean(nullable: false));
            AddColumn("dbo.Communities", "canCreateEvents", c => c.Boolean(nullable: false));
            AddColumn("dbo.Communities", "GroupPhoto", c => c.String());
            AddColumn("dbo.Communities", "AdminId", c => c.String());
            DropTable("dbo.Groups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        isOpen = c.Boolean(nullable: false),
                        isHiden = c.Boolean(nullable: false),
                        canCreateEvents = c.Boolean(nullable: false),
                        GroupPhoto = c.String(),
                        AdminId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Communities", "AdminId");
            DropColumn("dbo.Communities", "GroupPhoto");
            DropColumn("dbo.Communities", "canCreateEvents");
            DropColumn("dbo.Communities", "isHiden");
            DropColumn("dbo.Communities", "isOpen");
        }
    }
}
