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
            Mapper.Map(numberRemaining, viewModel);

            return viewModel;
        }

        public void CreateTicket(TicketViewModel ticketViewModel)
        {
            Ticket ticket = new Ticket
            {
                ConfirmationNumber = confirmationNumberUtil.CreateConfirmationNumber(ticketViewModel),
                Recipient = ticketViewModel.Ticket.Recipient,
                Email = ticketViewModel.Ticket.Email
            };

            SeatCapacity capacity = new SeatCapacity();
            Mapper.Map(ticketViewModel, capacity);
            
            IEnumerable<Reservation> reservations = reservationService.CreateReservations(capacity);

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
    }
}
