namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTopicGenrePerformerTypeToEnums : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Performers", "GenreId");
            DropColumn("dbo.Performers", "TopicId");
            DropColumn("dbo.Performers", "PerformerTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Performers", "PerformerTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Performers", "TopicId", c => c.Int());
            AddColumn("dbo.Performers", "GenreId", c => c.Int());
        }
    }
}
