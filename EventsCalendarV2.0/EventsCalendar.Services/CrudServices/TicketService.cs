using AutoMapper;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;
using EventsCalendar.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCalendar.Services.CrudServices
{
    public class TicketService : ITicketService
    {
        private readonly IGuidRepository<Ticket> _repository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IRepository<Performance> _performanceRepository;
        private readonly ConfirmationNumberUtil confirmationNumberUtil;
        private readonly IReservationService reservationService;

        public TicketService(IGuidRepository<Ticket> repository,
                             IReservationRepository reservationRepository,
                             IRepository<Seat> seatRepository,
                             IRepository<Performance> performanceRepository,
                             ConfirmationNumberUtil _confirmationNumberUtil,
                             IReservationService _reservationService)
        {
            _repository = repository;
            _reservationRepository = reservationRepository;
            _seatRepository = seatRepository;
            _performanceRepository = performanceRepository;
            confirmationNumberUtil = _confirmationNumberUtil;
            reservationService = _reservationService;
        }

        public IEnumerable<TicketViewModel> ListTickets()
        {
            throw new NotImplementedException();
        }

        public NewTicketViewModel NewTicketViewModel(int performanceId)
        {
            var prices = _reservationRepository.GetPrices(performanceId);

            NewTicketViewModel viewModel = new NewTicketViewModel
            {
                PriceOfBudget = prices.Budget,
                PriceOfModerate = prices.Moderate,
                PriceOfPremier = prices.Premier
            };

            SeatCapacity numberRemaining = reservationService.GetSeatsRemaining(performanceId);
            viewModel.Ticket.NumberOfBudget = numberRemaining.Budget;
            viewModel.Ticket.NumberOfModerate = numberRemaining.Moderate;
            viewModel.Ticket.NumberOfPremier = numberRemaining.Premier;

            return viewModel;
        }

        public void CreateTicket(TicketViewModel ticketViewModel)
        {
            var performerName = ticketViewModel.Ticket.Reservations.FirstOrDefault().Performance.PerformerDto.Name;
            var seatId = ticketViewModel.Ticket.Reservations.FirstOrDefault().SeatId.ToString();
            var venueName = ticketViewModel.Ticket.Reservations.FirstOrDefault().Seat.VenueDto.Name;
            var reservationNumber = ticketViewModel.Ticket.Reservations.FirstOrDefault().Id.ToString();
            Random random = new Random();

            ConfirmationNumberData data = new ConfirmationNumberData
            {
                PerformerChar = performerName[0],
                SeatDigit = seatId[seatId.Length - 1],
                VenueChar = venueName[0],
                VenueRandom = venueName[random.Next(venueName.Length)],
                FirstReservationChar = reservationNumber[0],
                LastReservationChar = reservationNumber[reservationNumber.Length - 1],
                RandomReservationChar = reservationNumber[random.Next(reservationNumber.Length)]
            };

            Ticket ticket = new Ticket
            {
                ConfirmationNumber = confirmationNumberUtil.CreateConfirmationNumber(data),
                Recipient = ticketViewModel.Ticket.Recipient,
                Email = ticketViewModel.Ticket.Email
            };

            IEnumerable<Reservation> reservations = reservationService.CreateReservations(
                ticketViewModel.NumberOfBudget,
                ticketViewModel.NumberOfModerate,
                ticketViewModel.NumberOfPremier);

            foreach (var reservation in reservations)
            {
                ticket.TotalPrice += reservation.Price;
                reservation.IsTaken = true;
            }

            _repository.Insert(ticket);
            _repository.Commit();
        }

        public TicketViewModel ReturnTicketViewModel(Guid id)
        {
            throw new NotImplementedException();
        }

        public void EditTicket(TicketViewModel ticketViewModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteTicket(Guid id)
        {
            throw new NotImplementedException();
        }

        TicketViewModel ITicketService.NewTicketViewModel()
        {
            throw new NotImplementedException();
        }
    }
}
