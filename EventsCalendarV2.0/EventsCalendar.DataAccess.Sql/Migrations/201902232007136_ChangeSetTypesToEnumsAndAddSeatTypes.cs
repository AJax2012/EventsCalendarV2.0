namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSetTypesToEnumsAndAddSeatTypes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Performers", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.Performers", "PerformerTypeId", "dbo.PerformerTypes");
            DropForeignKey("dbo.Performers", "TopicId", "dbo.Topics");
            DropIndex("dbo.Performers", new[] { "GenreId" });
            DropIndex("dbo.Performers", new[] { "TopicId" });
            DropIndex("dbo.Performers", new[] { "PerformerTypeId" });
            CreateTable(
                "dbo.SeatTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeatTypeLevels = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Performers", "Genre", c => c.Int(nullable: false));
            AddColumn("dbo.Performers", "Topic", c => c.Int(nullable: false));
            AddColumn("dbo.Performers", "PerformerType", c => c.Int(nullable: false));
            AddColumn("dbo.Seats", "PerformanceId", c => c.Int(nullable: false));
            AddColumn("dbo.Seats", "SeatType_Id", c => c.Int());
            CreateIndex("dbo.Seats", "PerformanceId");
            CreateIndex("dbo.Seats", "SeatType_Id");
            AddForeignKey("dbo.Seats", "PerformanceId", "dbo.Performances", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Seats", "SeatType_Id", "dbo.SeatTypes", "Id");
            DropColumn("dbo.Performances", "Price");
            DropColumn("dbo.Seats", "SeatType");
            DropTable("dbo.Genres");
            DropTable("dbo.PerformerTypes");
            DropTable("dbo.Topics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PerformerTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Seats", "SeatType", c => c.Int(nullable: false));
            AddColumn("dbo.Performances", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.Seats", "SeatType_Id", "dbo.SeatTypes");
            DropForeignKey("dbo.Seats", "PerformanceId", "dbo.Performances");
            DropIndex("dbo.Seats", new[] { "SeatType_Id" });
            DropIndex("dbo.Seats", new[] { "PerformanceId" });
            DropColumn("dbo.Seats", "SeatType_Id");
            DropColumn("dbo.Seats", "PerformanceId");
            DropColumn("dbo.Performers", "PerformerType");
            DropColumn("dbo.Performers", "Topic");
            DropColumn("dbo.Performers", "Genre");
            DropTable("dbo.SeatTypes");
            CreateIndex("dbo.Performers", "PerformerTypeId");
            CreateIndex("dbo.Performers", "TopicId");
            CreateIndex("dbo.Performers", "GenreId");
            AddForeignKey("dbo.Performers", "TopicId", "dbo.Topics", "Id");
            AddForeignKey("dbo.Performers", "PerformerTypeId", "dbo.PerformerTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Performers", "GenreId", "dbo.Genres", "Id");
        }
    }
}
