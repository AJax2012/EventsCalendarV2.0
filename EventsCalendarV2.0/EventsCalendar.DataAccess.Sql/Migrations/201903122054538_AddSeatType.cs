namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeatType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeatTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Seats", "SeatTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Seats", "SeatTypeId");
            AddForeignKey("dbo.Seats", "SeatTypeId", "dbo.SeatTypes", "Id", cascadeDelete: false);
            DropColumn("dbo.Seats", "SeatType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Seats", "SeatType", c => c.Int(nullable: false));
            DropForeignKey("dbo.Seats", "SeatTypeId", "dbo.SeatTypes");
            DropIndex("dbo.Seats", new[] { "SeatTypeId" });
            DropColumn("dbo.Seats", "SeatTypeId");
            DropTable("dbo.SeatTypes");
        }
    }
}
