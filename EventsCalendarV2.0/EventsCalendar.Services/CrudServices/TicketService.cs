using AutoMapper;
using EventsCalendar.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Core.Models.Tickets;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Contracts.Services;

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

        public IEnumerable<ITicketViewModel> ListTickets(ITicketViewModel viewModel)
        {
            IEnumerable<Ticket> tickets = _repository.Collection().ToList();
            return tickets.Select(ticket => Mapper.Map(ticket, viewModel)).ToList();
        }

        public ITicketViewModel NewTicketViewModel(ITicketViewModel viewModel)
        {
            ReservationPrices prices = _reservationRepository.GetPrices(viewModel.PerformanceId);
            SeatCapacityDto numberRemaining = _reservationService.GetSeatsRemaining(viewModel.PerformanceId);

            viewModel.BudgetPrice = prices.Budget;
            viewModel.ModeratePrice = prices.Moderate;
            viewModel.PremierPrice = prices.Premier;
            Mapper.Map(numberRemaining, viewModel);
            

            return viewModel;
        }

        public void CreateTicket(ITicketViewModel ticketViewModel)
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

        public ITicketViewModel ReturnTicketViewModelById(Guid id)
        {
            Ticket ticket = CheckTicketNullValueById(id);
            return Mapper.Map<Ticket, ITicketViewModel>(ticket);
        }

        public ITicketViewModel ReturnTicketViewModelByConfirmationNumber(string confirmationNumber)
        {
            Ticket ticket = CheckTicketNullByConfirmationNumber(confirmationNumber);
            return Mapper.Map<Ticket, ITicketViewModel>(ticket);
        }

        public void EditTicket(ITicketViewModel ticketViewModel)
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
