using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EventsCalendar.Core.Models;

namespace EventsCalendar.EntityConfigurations
{
    public class VenueConfiguration : EntityTypeConfiguration<Venue>
    {
        public VenueConfiguration()
        {
            HasIndex(v => v.Id)
                .IsUnique();

            Property(v => v.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(v => v.IsActive)
                .IsRequired();

            Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(v => v.ImageUrl)
                .IsRequired();

            HasRequired(v => v.Address)
                .WithMany()
                .HasForeignKey(v => v.AddressId)
                .WillCascadeOnDelete(true);

            HasMany(v => v.Performances)
                .WithRequired(p => p.Venue)
                .HasForeignKey(p => p.VenueId)
                .WillCascadeOnDelete(false);

            HasMany(v => v.Seats)
                .WithRequired(s => s.Venue)
                .HasForeignKey(s => s.VenueId)
                .WillCascadeOnDelete(false);
        }
    }
}
