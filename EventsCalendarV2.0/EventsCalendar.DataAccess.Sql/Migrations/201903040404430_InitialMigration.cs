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
                "dbo.Performances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        Genre = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        Topic = c.Int(nullable: false),
                        TopicId = c.Int(),
                        PerformerTypeId = c.Int(nullable: false),
                        PerformerType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SeatId = c.Int(nullable: false),
                        PerformanceId = c.Int(nullable: false),
                        IsTaken = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Performances", t => t.PerformanceId, cascadeDelete: true)
                .ForeignKey("dbo.Seats", t => t.SeatId, cascadeDelete: true)
                .Index(t => t.SeatId)
                .Index(t => t.PerformanceId);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeatTypeId = c.Int(nullable: false),
                        VenueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SeatTypes", t => t.SeatTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.VenueId, cascadeDelete: false)
                .Index(t => t.SeatTypeId)
                .Index(t => t.VenueId);
            
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
            
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
            DropForeignKey("dbo.Seats", "VenueId", "dbo.Venues");
            DropForeignKey("dbo.Performances", "VenueId", "dbo.Venues");
            DropForeignKey("dbo.Venues", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Seats", "SeatTypeId", "dbo.SeatTypes");
            DropForeignKey("dbo.Reservations", "SeatId", "dbo.Seats");
            DropForeignKey("dbo.Reservations", "PerformanceId", "dbo.Performances");
            DropForeignKey("dbo.Performances", "PerformerId", "dbo.Performers");
            DropIndex("dbo.Venues", new[] { "AddressId" });
            DropIndex("dbo.Seats", new[] { "VenueId" });
            DropIndex("dbo.Seats", new[] { "SeatTypeId" });
            DropIndex("dbo.Reservations", new[] { "PerformanceId" });
            DropIndex("dbo.Reservations", new[] { "SeatId" });
            DropIndex("dbo.Performances", new[] { "VenueId" });
            DropIndex("dbo.Performances", new[] { "PerformerId" });
            DropTable("dbo.Venues");
            DropTable("dbo.SeatTypes");
            DropTable("dbo.Seats");
            DropTable("dbo.Reservations");
            DropTable("dbo.Performers");
            DropTable("dbo.Performances");
            DropTable("dbo.Addresses");
        }
    }
}
