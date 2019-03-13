﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.EntityConfigurations
{
    public class SeatConfiguration : EntityTypeConfiguration<Seat>
    {
        public SeatConfiguration()
        {
            HasIndex(s => s.Id)
                .IsUnique();

            Property(s => s.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            HasRequired(s => s.SeatType)
                .WithMany()
                .HasForeignKey(s => s.SeatTypeId)
                .WillCascadeOnDelete(false);

            HasMany(s => s.Reservations)
                .WithRequired(r => r.Seat)
                .HasForeignKey(r => r.SeatId)
                .WillCascadeOnDelete(false);
        }
    }
}
