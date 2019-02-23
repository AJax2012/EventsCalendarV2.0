namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSeats : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Seats", "PerformanceId", "dbo.Performances");
            DropIndex("dbo.Seats", new[] { "PerformanceId" });
            CreateTable(
                "dbo.SeatPerformances",
                c => new
                    {
                        Seat_Id = c.Int(nullable: false),
                        Performance_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Seat_Id, t.Performance_Id })
                .ForeignKey("dbo.Seats", t => t.Seat_Id, cascadeDelete: false)
                .ForeignKey("dbo.Performances", t => t.Performance_Id, cascadeDelete: false)
                .Index(t => t.Seat_Id)
                .Index(t => t.Performance_Id);
            
            AddColumn("dbo.Seats", "VenueId", c => c.Int(nullable: false));
            AddColumn("dbo.SeatTypes", "NumberOfSeats", c => c.Int(nullable: false));
            AlterColumn("dbo.SeatTypes", "Price", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.Seats", "VenueId");
            AddForeignKey("dbo.Seats", "VenueId", "dbo.Venues", "Id", cascadeDelete: true);
            DropColumn("dbo.Seats", "PerformanceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Seats", "PerformanceId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Seats", "VenueId", "dbo.Venues");
            DropForeignKey("dbo.SeatPerformances", "Performance_Id", "dbo.Performances");
            DropForeignKey("dbo.SeatPerformances", "Seat_Id", "dbo.Seats");
            DropIndex("dbo.SeatPerformances", new[] { "Performance_Id" });
            DropIndex("dbo.SeatPerformances", new[] { "Seat_Id" });
            DropIndex("dbo.Seats", new[] { "VenueId" });
            AlterColumn("dbo.SeatTypes", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.SeatTypes", "NumberOfSeats");
            DropColumn("dbo.Seats", "VenueId");
            DropTable("dbo.SeatPerformances");
            CreateIndex("dbo.Seats", "PerformanceId");
            AddForeignKey("dbo.Seats", "PerformanceId", "dbo.Performances", "Id", cascadeDelete: false);
        }
    }
}
