using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Services.Dtos.Performer
{
    public enum PerformerTypeDto
    {
        Musician = 1,

        [Display(Name = "Public Speaker")]
        PublicSpeaker
    }
}