namespace EventsCalendar.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTicketIDtoNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservations", "TicketId", "dbo.Tickets");
            DropIndex("dbo.Reservations", new[] { "TicketId" });
            AlterColumn("dbo.Reservations", "TicketId", c => c.Guid());
            CreateIndex("dbo.Reservations", "TicketId");
            AddForeignKey("dbo.Reservations", "TicketId", "dbo.Tickets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "TicketId", "dbo.Tickets");
            DropIndex("dbo.Reservations", new[] { "TicketId" });
            AlterColumn("dbo.Reservations", "TicketId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Reservations", "TicketId");
            AddForeignKey("dbo.Reservations", "TicketId", "dbo.Tickets", "Id", cascadeDelete: true);
        }
    }
}
