namespace kursach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCardInformationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardInformation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Description = c.String(nullable: false, maxLength: 500),
                        ImageUrl = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CardInformation");
        }
    }
}
