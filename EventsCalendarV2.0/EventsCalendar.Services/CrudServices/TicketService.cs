using AutoMapper;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.ViewModels;
using EventsCalendar.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventsCalendar.Core.Contracts.Repositories;
using EventsCalendar.Core.Contracts.Services;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Core.Models.Tickets;

namespace EventsCalendar.Services.CrudServices
{
    public class TicketService : ITicketService
    {
        private readonly IGuidRepository<Ticket> _repository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ConfirmationNumberUtil _confirmationNumberUtil;
        private readonly IReservationService _reservationService;
        private readonly ISeatService _seatService;

        public TicketService(IGuidRepository<Ticket> repository,
                             IReservationRepository reservationRepository,
                             ConfirmationNumberUtil confirmationNumberUtil,
                             IReservationService reservationService,
                             ISeatService seatService)
        {
            _repository = repository;
            _reservationRepository = reservationRepository;
            _confirmationNumberUtil = confirmationNumberUtil;
            _reservationService = reservationService;
            _seatService = seatService;
        }

        private Ticket CheckTicketNullValue(Guid id)
        {
            Ticket ticket = _repository.Find(id);
            if (ticket == null)
                throw new HttpException(404, "Ticket Not Found");

            return ticket;
        }

        //private Ticket CheckTicketNullByConfirmationNumber(string confirmationNumber)
        //{
        //    Ticket ticket = _repository.Find(id);
        //    if (ticket == null)
        //        throw new HttpException(404, "Ticket Not Found");

        //    return ticket;
        //}

        public IEnumerable<TicketViewModel> ListTickets()
        {
            throw new NotImplementedException();
        }

        public NewTicketViewModel NewTicketViewModel(int performanceId)
        {
            ReservationPrices prices = _reservationRepository.GetPrices(performanceId);

            var viewModel = new NewTicketViewModel
            {
                PriceOfBudget = prices.Budget,
                PriceOfModerate = prices.Moderate,
                PriceOfPremier = prices.Premier
            };

            SeatCapacity numberRemaining = _reservationService.GetSeatsRemaining(performanceId);
            Mapper.Map(numberRemaining, viewModel);

            return viewModel;
        }

        public void CreateTicket(TicketViewModel ticketViewModel)
        {
            var ticket = new Ticket
            {
                ConfirmationNumber = _confirmationNumberUtil.CreateConfirmationNumber(ticketViewModel),
                Recipient = ticketViewModel.Ticket.Recipient,
                Email = ticketViewModel.Ticket.Email,
            };

            var capacity = new SeatCapacity();
            Mapper.Map(ticketViewModel, capacity);
            
            IEnumerable<Reservation> reservations = _reservationService.GetReservations(capacity, ticketViewModel.PerformanceId);

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
            Ticket ticket = CheckTicketNullValue(id);

            var seatsInTicket = ticket.Reservations
                .Where(res => res.TicketId == id)
                .ToList();

            var viewModel = new TicketViewModel
            {
                Ticket = Mapper.Map<Ticket, TicketDto>(ticket),

                NumberOfBudget = seatsInTicket
                    .Count(s => s.Seat.SeatType == SeatType.Budget),

                NumberOfModerate = seatsInTicket
                    .Count(s => s.Seat.SeatType == SeatType.Moderate),

                NumberOfPremier = seatsInTicket
                .Count(s => s.Seat.SeatType == SeatType.Premier)
            };

            return viewModel;
        }

//        public TicketViewModel ReturnTicketViewModelByConfirmationNumber(string confirmationNumber)
//        {
//            Ticket ticket = CheckTicketNullByConfirmationNumber(confirmationNumber);
//            ticket.
//        }

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
