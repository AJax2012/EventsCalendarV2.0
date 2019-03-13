using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EventsCalendar.Core.Models;

namespace EventsCalendar.EntityConfigurations
{
    public class PerformerTypeConfiguration : EntityTypeConfiguration<PerformerType>
    {
        public PerformerTypeConfiguration()
        {
            Property(p => p.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(p => p.Name)
                .IsRequired();
        }
    }
}
