namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class community2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communities", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communities", "IsActive");
        }
    }
}
