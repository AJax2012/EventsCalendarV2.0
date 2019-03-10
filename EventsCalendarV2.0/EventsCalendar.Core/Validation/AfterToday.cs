using System;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Validation
{
    public class AfterToday : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var performance = (PerformanceViewModel) validationContext.ObjectInstance;

            DateTime.TryParse(performance.EventDate, out var eventDate);

            var result = DateTime.Compare(eventDate, DateTime.Today);

            return result >= 0 ? ValidationResult.Success
                : new ValidationResult("Please submit a value later than current date/time");
        }
    }
}
