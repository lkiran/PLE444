namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chapterOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "OrderBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chapters", "OrderBy");
        }
    }
}
