namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCapacitiesFromPerformance : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Performances", "BudgetSeatsRemaining");
            DropColumn("dbo.Performances", "ModerateSeatsRemaining");
            DropColumn("dbo.Performances", "PremierSeatsRemaining");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Performances", "PremierSeatsRemaining", c => c.Int(nullable: false));
            AddColumn("dbo.Performances", "ModerateSeatsRemaining", c => c.Int(nullable: false));
            AddColumn("dbo.Performances", "BudgetSeatsRemaining", c => c.Int(nullable: false));
        }
    }
}
