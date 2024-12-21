namespace kursach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFullCardInformationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardInformation", "Rental_Period", c => c.String(maxLength: 50));
            AddColumn("dbo.CardInformation", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CardInformation", "Location", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardInformation", "Location");
            DropColumn("dbo.CardInformation", "Price");
            DropColumn("dbo.CardInformation", "Rental_Period");
        }
    }
}
