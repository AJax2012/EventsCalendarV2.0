using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql;
using System;
using System.Text;

namespace EventsCalendar.Services.Helpers
{
    public class ConfirmationNumberUtil
    {
        private readonly MsSqlTicketRepository _repository;
        public ConfirmationNumberUtil(MsSqlTicketRepository repository)
        {
            _repository = repository;
        }

        private static Random random = new Random();
        private readonly int numberLength = 10;

        public string CreateConfirmationNumber(ConfirmationNumberData data)
        {
            var datetime = DateTime.Now;

            string month = datetime.ToString("MMM", MonthFirstCharFormatter.FormatProvider);
            string date = datetime.ToString("dd");
            string year = datetime.ToString("yy");
            string hour = datetime.ToString("HH");

            StringBuilder confirmationNumber = new StringBuilder();
            confirmationNumber.Append(data.PerformerChar);
            confirmationNumber.Append(date);
            confirmationNumber.Append(data.VenueChar);
            confirmationNumber.Append(hour);
            confirmationNumber.Append(data.LastReservationChar);
            confirmationNumber.Append(data.SeatDigit);
            confirmationNumber.Append(data.FirstReservationChar);
            confirmationNumber.Append(year);
            confirmationNumber.Append(data.RandomReservationChar);
            confirmationNumber.Append(data.VenueRandom);
            
            var stringConfNumb = confirmationNumber.ToString();

            return stringConfNumb;
        }
    }
}
