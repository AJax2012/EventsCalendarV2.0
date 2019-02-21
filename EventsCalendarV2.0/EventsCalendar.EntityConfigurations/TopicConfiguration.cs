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
    public class TopicConfiguration : EntityTypeConfiguration<Topic>
    {
        public TopicConfiguration()
        {
            HasKey(p => p.Id);

            HasIndex(t => t.Name)
                .IsUnique();

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
