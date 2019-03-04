namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSeatType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Seats", "SeatTypeId", "dbo.SeatTypes");
            DropIndex("dbo.Seats", new[] { "SeatTypeId" });
            AddColumn("dbo.Seats", "SeatType", c => c.Int(nullable: false));
            DropColumn("dbo.Seats", "SeatTypeId");
            DropTable("dbo.SeatTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SeatTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeatTypeLevel = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        NumberOfSeats = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Seats", "SeatTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.Seats", "SeatType");
            CreateIndex("dbo.Seats", "SeatTypeId");
            AddForeignKey("dbo.Seats", "SeatTypeId", "dbo.SeatTypes", "Id", cascadeDelete: true);
        }
    }
}
