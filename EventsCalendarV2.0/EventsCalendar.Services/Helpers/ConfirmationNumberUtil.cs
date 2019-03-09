using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string datetime = new DateTime().ToString("MMM dd yy HH");

            char month = datetime[0];
            string date = datetime.Split(' ')[1];
            string year = datetime.Split(' ')[2];
            string hour = datetime.Split(' ')[3];

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
