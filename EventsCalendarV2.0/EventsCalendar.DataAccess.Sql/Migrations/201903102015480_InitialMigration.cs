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
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        EventDateTime = c.DateTime(nullable: false),
                        PerformerId = c.Int(nullable: false),
                        VenueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Performers", t => t.PerformerId, cascadeDelete: false)
                .ForeignKey("dbo.Venues", t => t.VenueId, cascadeDelete: false)
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
                        ImageUrl = c.String(),
                        Genre = c.Int(nullable: false),
                        Topic = c.Int(nullable: false),
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
                        TicketId = c.Guid(nullable: false),
                        IsTaken = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Performances", t => t.PerformanceId, cascadeDelete: false)
                .ForeignKey("dbo.Seats", t => t.SeatId, cascadeDelete: false)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: false)
                .Index(t => t.SeatId)
                .Index(t => t.PerformanceId)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeatType = c.Int(nullable: false),
                        VenueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Venues", t => t.VenueId, cascadeDelete: false)
                .Index(t => t.VenueId);
            
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
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: false)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConfirmationNumber = c.String(),
                        Recipient = c.String(),
                        Email = c.String(),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.Seats", "VenueId", "dbo.Venues");
            DropForeignKey("dbo.Performances", "VenueId", "dbo.Venues");
            DropForeignKey("dbo.Venues", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Reservations", "SeatId", "dbo.Seats");
            DropForeignKey("dbo.Reservations", "PerformanceId", "dbo.Performances");
            DropForeignKey("dbo.Performances", "PerformerId", "dbo.Performers");
            DropIndex("dbo.Venues", new[] { "AddressId" });
            DropIndex("dbo.Seats", new[] { "VenueId" });
            DropIndex("dbo.Reservations", new[] { "TicketId" });
            DropIndex("dbo.Reservations", new[] { "PerformanceId" });
            DropIndex("dbo.Reservations", new[] { "SeatId" });
            DropIndex("dbo.Performances", new[] { "VenueId" });
            DropIndex("dbo.Performances", new[] { "PerformerId" });
            DropTable("dbo.Tickets");
            DropTable("dbo.Venues");
            DropTable("dbo.Seats");
            DropTable("dbo.Reservations");
            DropTable("dbo.Performers");
            DropTable("dbo.Performances");
            DropTable("dbo.Addresses");
        }
    }
}
