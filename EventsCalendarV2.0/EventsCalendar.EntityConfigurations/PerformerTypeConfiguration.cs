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
    public class PerformerTypeConfiguration : EntityTypeConfiguration<PerformerType>
    {
        public PerformerTypeConfiguration()
        {
            HasKey(p => p.Id);

            HasIndex(p => p.Name)
                .IsUnique();

            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
