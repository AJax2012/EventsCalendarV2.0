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
    public class GenreConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreConfiguration()
        {
            HasKey(p => p.Id);

            HasIndex(g => g.Name)
                .IsUnique();

            Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
