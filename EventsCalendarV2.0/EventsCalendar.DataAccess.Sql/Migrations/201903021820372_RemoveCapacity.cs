namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCapacity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Venues", "Capacity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Venues", "Capacity", c => c.Int(nullable: false));
        }
    }
}
