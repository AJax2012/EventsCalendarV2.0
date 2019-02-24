namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reservations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SeatPerformances", "Seat_Id", "dbo.Seats");
            DropForeignKey("dbo.SeatPerformances", "Performance_Id", "dbo.Performances");
            DropIndex("dbo.SeatPerformances", new[] { "Seat_Id" });
            DropIndex("dbo.SeatPerformances", new[] { "Performance_Id" });
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SeatId = c.Int(nullable: false),
                        PerformanceId = c.Int(nullable: false),
                        IsTaken = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Performances", t => t.PerformanceId, cascadeDelete: true)
                .ForeignKey("dbo.Seats", t => t.SeatId, cascadeDelete: false)
                .Index(t => t.SeatId)
                .Index(t => t.PerformanceId);
            
            DropColumn("dbo.Seats", "Price");
            DropTable("dbo.SeatPerformances");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SeatPerformances",
                c => new
                    {
                        Seat_Id = c.Int(nullable: false),
                        Performance_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Seat_Id, t.Performance_Id });
            
            AddColumn("dbo.Seats", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.Reservations", "SeatId", "dbo.Seats");
            DropForeignKey("dbo.Reservations", "PerformanceId", "dbo.Performances");
            DropIndex("dbo.Reservations", new[] { "PerformanceId" });
            DropIndex("dbo.Reservations", new[] { "SeatId" });
            DropTable("dbo.Reservations");
            CreateIndex("dbo.SeatPerformances", "Performance_Id");
            CreateIndex("dbo.SeatPerformances", "Seat_Id");
            AddForeignKey("dbo.SeatPerformances", "Performance_Id", "dbo.Performances", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SeatPerformances", "Seat_Id", "dbo.Seats", "Id", cascadeDelete: true);
        }
    }
}
