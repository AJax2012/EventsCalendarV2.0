namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNameOfSeatTypeLevels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeatTypes", "SeatTypeLevel", c => c.Int(nullable: false));
            DropColumn("dbo.SeatTypes", "SeatTypeLevels");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SeatTypes", "SeatTypeLevels", c => c.Int(nullable: false));
            DropColumn("dbo.SeatTypes", "SeatTypeLevel");
        }
    }
}
