using System.Data.Entity;
using EventsCalendar.Services;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventsCalendar.WebUI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly BuildModelBuilderConfigurations _builder = new BuildModelBuilderConfigurations();

        public ApplicationDbContext()
            : base("EventCalendarDataContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _builder.Builder(modelBuilder);
        }
    }
}