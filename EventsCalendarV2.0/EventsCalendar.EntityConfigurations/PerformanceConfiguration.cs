using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsCalendar.Core.Models;

namespace EventsCalendar.EntityConfigurations
{
    public class PerformanceConfiguration : EntityTypeConfiguration<Performance>
    {
        public PerformanceConfiguration()
        {
            Property(p => p.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(p => p.IsActive)
                .IsRequired();

            Property(p => p.EventDateTime)
                .IsRequired();

            Property(p => p.Price)
                .HasPrecision(6, 2);

            HasRequired(p => p.Venue)
                .WithMany(v => v.Performances)
                .HasForeignKey(p => p.VenueId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Performer)
                .WithMany(p => p.Performances)
                .HasForeignKey(p => p.PerformerId)
                .WillCascadeOnDelete(false);
        }
    }
}
