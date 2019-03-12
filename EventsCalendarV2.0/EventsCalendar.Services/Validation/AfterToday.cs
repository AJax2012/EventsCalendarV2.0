using System;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Services.Validation
{
    public class AfterToday : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime;

            try
            {
                dateTime = Convert.ToDateTime(value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return DateTime.Compare(dateTime, DateTime.Today) >= 0;
        }
    }
}
