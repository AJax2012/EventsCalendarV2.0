using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Models
{
    public enum Genre
    {
        Rock,

        Classical,

        [Display(Name = "Classic Rock")]
        ClassicRock,

        Jazz,

        Blues,

        Alternative
    }
}