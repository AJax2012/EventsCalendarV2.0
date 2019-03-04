using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Models
{
    public enum Genre
    {
        Rock,

        Classical,

        [Display(Name = "Classic Rock")]
        Classic_Rock,

        Jazz,

        Blues,

        Alternative
    }
}