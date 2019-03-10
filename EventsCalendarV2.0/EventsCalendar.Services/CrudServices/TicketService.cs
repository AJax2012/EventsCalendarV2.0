using AutoMapper;
using EventsCalendar.Core.ViewModels;
using EventsCalendar.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventsCalendar.Core.Contracts.Repositories;
using EventsCalendar.Core.Contracts.Services;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Core.Models.Tickets;

namespace EventsCalendar.Services.CrudServices
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ConfirmationNumberUtil _confirmationNumberUtil;
        private readonly IReservationService _reservationService;

        public TicketService(ITicketRepository repository,
                             IReservationRepository reservationRepository,
                             ConfirmationNumberUtil confirmationNumberUtil,
                             IReservationService reservationService)
        {
            _repository = repository;
            _reservationRepository = reservationRepository;
            _confirmationNumberUtil = confirmationNumberUtil;
            _reservationService = reservationService;
        }

        private Ticket CheckTicketNullValueById(Guid id)
        {
            Ticket ticket = _repository.Find(id);
            if (ticket == null)
                throw new HttpException(404, "Ticket Not Found");

            return ticket;
        }

        private Ticket CheckTicketNullByConfirmationNumber(string confirmationNumber)
        {
            Ticket ticket = _repository.FindByConfirmationNumber(confirmationNumber);
            if (ticket == null)
                throw new HttpException(404, "Ticket Not Found");

            return ticket;
        }

        private TicketViewModel MapTicketToViewModel(Ticket ticket)
        {
            var reservations = ticket.Reservations;

            return new TicketViewModel
            {
                Ticket = Mapper.Map<Ticket, TicketDto>(ticket),
                PerformanceId = reservations.First().PerformanceId,
                SeatCapacity =
                {
                    Budget = reservations.Count(r => r.Seat.SeatType == SeatType.Budget),
                    Moderate = reservations.Count(r => r.Seat.SeatType == SeatType.Moderate),
                    Premier = reservations.Count(r => r.Seat.SeatType == SeatType.Premier)
                }
            };
        }

        public IEnumerable<TicketViewModel> ListTickets()
        {
            IEnumerable<Ticket> tickets = _repository.Collection().ToList();
            return tickets.Select(MapTicketToViewModel).ToList();
        }

        public TicketViewModel NewTicketViewModel(int performanceId)
        {
            ReservationPrices prices = _reservationRepository.GetPrices(performanceId);

            var viewModel = new TicketViewModel
            {
                BudgetPrice = prices.Budget,
                ModeratePrice = prices.Moderate,
                PremierPrice = prices.Premier
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

        public TicketViewModel ReturnTicketViewModelById(Guid id)
        {
            Ticket ticket = CheckTicketNullValueById(id);
            var viewModel = MapTicketToViewModel(ticket);
            return viewModel;
        }

        public TicketViewModel ReturnTicketViewModelByConfirmationNumber(string confirmationNumber)
        {
            Ticket ticket = CheckTicketNullByConfirmationNumber(confirmationNumber);
            var viewModel = MapTicketToViewModel(ticket);
            return viewModel;
        }

        public void EditTicket(TicketViewModel ticketViewModel)
        {
            Ticket ticketToEdit = CheckTicketNullValueById(ticketViewModel.Ticket.Id);

            ticketToEdit.Email = ticketViewModel.Ticket.Email;
            ticketToEdit.Recipient = ticketViewModel.Ticket.Recipient;
            Mapper.Map(ticketViewModel.Ticket.Reservations, ticketToEdit.Reservations);

            _repository.Commit();
        }

        public void DeleteTicket(Guid id)
        {
            CheckTicketNullValueById(id);
            _repository.Delete(id);
            _repository.Commit();
        }
    }
}
