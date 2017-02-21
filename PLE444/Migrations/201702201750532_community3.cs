namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class community3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCommunities", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserCommunities", "IsActive");
        }
    }
}
