namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveImageUpload : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Performers", "CustomImageId", "dbo.CustomImages");
            DropIndex("dbo.Performers", new[] { "CustomImageId" });
            AddColumn("dbo.Performers", "ImageUrl", c => c.String());
            DropColumn("dbo.Performers", "CustomImageId");
            DropTable("dbo.CustomImages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Performers", "CustomImageId", c => c.Int(nullable: false));
            DropColumn("dbo.Performers", "ImageUrl");
            CreateIndex("dbo.Performers", "CustomImageId");
            AddForeignKey("dbo.Performers", "CustomImageId", "dbo.CustomImages", "Id", cascadeDelete: true);
        }
    }
}
