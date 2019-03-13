using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Services.Dtos.Performer
{
    public enum GenreDto
    {
        Rock = 1,

        Classical,

        [Display(Name = "Classic Rock")]
        ClassicRock,

        Jazz,

        Blues,

        Alternative
    }
}