namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenreAndTopicsNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Performers", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.Performers", "TopicId", "dbo.Topics");
            DropIndex("dbo.Performers", new[] { "GenreId" });
            DropIndex("dbo.Performers", new[] { "TopicId" });
            AlterColumn("dbo.Performers", "GenreId", c => c.Int());
            AlterColumn("dbo.Performers", "TopicId", c => c.Int());
            CreateIndex("dbo.Performers", "GenreId");
            CreateIndex("dbo.Performers", "TopicId");
            AddForeignKey("dbo.Performers", "GenreId", "dbo.Genres", "Id");
            AddForeignKey("dbo.Performers", "TopicId", "dbo.Topics", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Performers", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Performers", "GenreId", "dbo.Genres");
            DropIndex("dbo.Performers", new[] { "TopicId" });
            DropIndex("dbo.Performers", new[] { "GenreId" });
            AlterColumn("dbo.Performers", "TopicId", c => c.Int(nullable: false));
            AlterColumn("dbo.Performers", "GenreId", c => c.Int(nullable: false));
            CreateIndex("dbo.Performers", "TopicId");
            CreateIndex("dbo.Performers", "GenreId");
            AddForeignKey("dbo.Performers", "TopicId", "dbo.Topics", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Performers", "GenreId", "dbo.Genres", "Id", cascadeDelete: true);
        }
    }
}
