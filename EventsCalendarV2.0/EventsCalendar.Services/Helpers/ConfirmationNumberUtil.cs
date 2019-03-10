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

        private readonly Random _random = new Random();
        private readonly int _numberLength = 10;

        public string CreateConfirmationNumber(TicketViewModel ticketViewModel)
        {
            var data = CreateConfirmationNumberData(ticketViewModel);
            var datetime = DateTime.Now;

            var month = datetime.ToString("MMM", MonthFirstCharFormatter.FormatProvider);
            var date = datetime.ToString("dd");
            var year = datetime.ToString("yy");
            var hour = datetime.ToString("HH");

            var confirmationNumber = new StringBuilder();
            confirmationNumber.Append(data.PerformerChar);
            confirmationNumber.Append(month);
            confirmationNumber.Append(data.VenueChar);
            confirmationNumber.Append(date);
            confirmationNumber.Append(data.LastReservationChar);
            confirmationNumber.Append(hour);
            confirmationNumber.Append(data.SeatDigit);
            confirmationNumber.Append(year);
            confirmationNumber.Append(data.FirstReservationChar);
            confirmationNumber.Append(data.RandomReservationChar);
            confirmationNumber.Append(data.VenueRandom);
            
            var stringConfNumb = confirmationNumber.ToString();

            return stringConfNumb;
        }

        private ConfirmationNumberData CreateConfirmationNumberData(TicketViewModel ticketViewModel)
        {
            var performerName = ticketViewModel.Ticket.Reservations.First().Performance.PerformerDto.Name;
            var seatId = ticketViewModel.Ticket.Reservations.First().SeatId.ToString();
            var venueName = ticketViewModel.Ticket.Reservations.First().Seat.VenueDto.Name;
            var reservationNumber = ticketViewModel.Ticket.Reservations.First().Id.ToString();

            return new ConfirmationNumberData
            {
                PerformerChar = performerName[0],
                SeatDigit = seatId[seatId.Length - 1],
                VenueChar = venueName[0],
                VenueRandom = venueName[_random.Next(venueName.Length)],
                FirstReservationChar = reservationNumber[0],
                LastReservationChar = reservationNumber[reservationNumber.Length - 1],
                RandomReservationChar = reservationNumber[_random.Next(reservationNumber.Length)]
            };
        }
    }
}
