using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;
using EventsCalendar.DataAccess.Sql;
using System;
using System.Linq;
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

        public string CreateConfirmationNumber(TicketViewModel ticketViewModel)
        {
            var data = CreateConfirmationNumberData(ticketViewModel);
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

        private ConfirmationNumberData CreateConfirmationNumberData(TicketViewModel ticketViewModel)
        {
            var performerName = ticketViewModel.Ticket.Reservations.FirstOrDefault().Performance.PerformerDto.Name;
            var seatId = ticketViewModel.Ticket.Reservations.FirstOrDefault().SeatId.ToString();
            var venueName = ticketViewModel.Ticket.Reservations.FirstOrDefault().Seat.VenueDto.Name;
            var reservationNumber = ticketViewModel.Ticket.Reservations.FirstOrDefault().Id.ToString();
            Random random = new Random();

            return new ConfirmationNumberData
            {
                PerformerChar = performerName[0],
                SeatDigit = seatId[seatId.Length - 1],
                VenueChar = venueName[0],
                VenueRandom = venueName[random.Next(venueName.Length)],
                FirstReservationChar = reservationNumber[0],
                LastReservationChar = reservationNumber[reservationNumber.Length - 1],
                RandomReservationChar = reservationNumber[random.Next(reservationNumber.Length)]
            };
        }
    }
}
