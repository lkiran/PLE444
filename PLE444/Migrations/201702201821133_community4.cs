namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class community4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communities", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communities", "DateCreated");
        }
    }
}
