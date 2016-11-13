namespace PLE444.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateudetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDetails", "PhoneNumber", c => c.String());
            AddColumn("dbo.UserDetails", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDetails", "Gender");
            DropColumn("dbo.UserDetails", "PhoneNumber");
        }
    }
}
