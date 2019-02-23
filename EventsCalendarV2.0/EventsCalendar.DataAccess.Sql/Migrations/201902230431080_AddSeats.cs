namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SeatType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Seats");
        }
    }
}
