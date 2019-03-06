namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditSeatsRemaining : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Performances", "BudgetSeatsRemaining", c => c.Int(nullable: false));
            AddColumn("dbo.Performances", "ModerateSeatsRemaining", c => c.Int(nullable: false));
            AddColumn("dbo.Performances", "PremierSeatsRemaining", c => c.Int(nullable: false));
            DropColumn("dbo.Performances", "SeatsRemaining");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Performances", "SeatsRemaining", c => c.Int(nullable: false));
            DropColumn("dbo.Performances", "PremierSeatsRemaining");
            DropColumn("dbo.Performances", "ModerateSeatsRemaining");
            DropColumn("dbo.Performances", "BudgetSeatsRemaining");
        }
    }
}
