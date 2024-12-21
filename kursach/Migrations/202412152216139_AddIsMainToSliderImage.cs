namespace kursach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsMainToSliderImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SliderImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(nullable: false, maxLength: 500),
                        Description = c.String(maxLength: 1000),
                        CardInformationId = c.Int(nullable: false),
                        IsMain = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardInformation", t => t.CardInformationId, cascadeDelete: true)
                .Index(t => t.CardInformationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SliderImages", "CardInformationId", "dbo.CardInformation");
            DropIndex("dbo.SliderImages", new[] { "CardInformationId" });
            DropTable("dbo.SliderImages");
        }
    }
}
