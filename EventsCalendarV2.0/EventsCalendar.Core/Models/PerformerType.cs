using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Models
{
    public sealed class PerformerType : BaseEntity
    {
        [Display(Name = "Performer Type")]
        public string Name { get; set; }

        public ICollection<Performer> Performers { get; set; }

        public PerformerType()
        {
            Performers = new List<Performer>();
        }
    }
}