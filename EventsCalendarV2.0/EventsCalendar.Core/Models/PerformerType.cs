using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Models
{
    public enum PerformerType
    {
        Musician,

        [Display(Name = "Public Speaker")]
        Public_Speaker
    }
}