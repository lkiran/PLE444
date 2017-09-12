namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chapters", "OrderBy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Chapters", "OrderBy", c => c.String());
        }
    }
}
