using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityConfiguration
{
    class EntityFluentApiConfiguration
    {
        protected void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new VenueConfiguration());
        }
    }
}
