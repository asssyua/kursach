namespace kursach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRequests", "UserBirthDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserRequests", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRequests", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserRequests", "UserBirthDate");
        }
    }
}
