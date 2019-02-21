namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Performances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SeatsRemaining = c.Int(nullable: false),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        EventDateTime = c.DateTime(nullable: false),
                        PerformerId = c.Int(nullable: false),
                        VenueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Performers", t => t.PerformerId, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.VenueId, cascadeDelete: true)
                .Index(t => t.PerformerId)
                .Index(t => t.VenueId);
            
            CreateTable(
                "dbo.Performers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TourName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        GenreId = c.Int(),
                        CustomImageId = c.Int(nullable: false),
                        TopicId = c.Int(),
                        PerformerTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomImages", t => t.CustomImageId, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.GenreId)
                .ForeignKey("dbo.PerformerTypes", t => t.PerformerTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Topics", t => t.TopicId)
                .Index(t => t.GenreId)
                .Index(t => t.CustomImageId)
                .Index(t => t.TopicId)
                .Index(t => t.PerformerTypeId);
            
            CreateTable(
                "dbo.CustomImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PerformerTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Capacity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        AddressId = c.Int(nullable: false),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Performances", "VenueId", "dbo.Venues");
            DropForeignKey("dbo.Venues", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Performers", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Performers", "PerformerTypeId", "dbo.PerformerTypes");
            DropForeignKey("dbo.Performances", "PerformerId", "dbo.Performers");
            DropForeignKey("dbo.Performers", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.Performers", "CustomImageId", "dbo.CustomImages");
            DropIndex("dbo.Venues", new[] { "AddressId" });
            DropIndex("dbo.Performers", new[] { "PerformerTypeId" });
            DropIndex("dbo.Performers", new[] { "TopicId" });
            DropIndex("dbo.Performers", new[] { "CustomImageId" });
            DropIndex("dbo.Performers", new[] { "GenreId" });
            DropIndex("dbo.Performances", new[] { "VenueId" });
            DropIndex("dbo.Performances", new[] { "PerformerId" });
            DropTable("dbo.Venues");
            DropTable("dbo.Topics");
            DropTable("dbo.PerformerTypes");
            DropTable("dbo.CustomImages");
            DropTable("dbo.Performers");
            DropTable("dbo.Performances");
            DropTable("dbo.Genres");
            DropTable("dbo.Addresses");
        }
    }
}
