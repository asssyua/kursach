namespace kursach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSliderImagesRelation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SliderImages", "Description");
            DropColumn("dbo.SliderImages", "IsMain");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SliderImages", "IsMain", c => c.Boolean(nullable: false));
            AddColumn("dbo.SliderImages", "Description", c => c.String(maxLength: 1000));
        }
    }
}
