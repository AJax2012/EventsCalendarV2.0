using AutoMapper;
using EventsCalendar.Services.Helpers;
using System;
using System.Linq;
using System.Web;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Seat;

namespace EventsCalendar.Services.CrudServices
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        private readonly ConfirmationNumberUtil _confirmationNumberUtil;
        private readonly IReservationService _reservationService;

        public TicketService(ITicketRepository repository,
                             ConfirmationNumberUtil confirmationNumberUtil,
                             IReservationService reservationService)
        {
            _repository = repository;
            _confirmationNumberUtil = confirmationNumberUtil;
            _reservationService = reservationService;
        }

        private Ticket CheckTicketNullValueById(Guid id)
        {
            var ticket = _repository.Find(id);
            if (ticket == null)
                throw new HttpException(404, "Ticket Not Found");

            return ticket;
        }

        private Ticket CheckTicketNullByConfirmationNumber(string confirmationNumber)
        {
            var ticket = _repository.FindByConfirmationNumber(confirmationNumber);
            if (ticket == null)
                throw new HttpException(404, "Ticket Not Found");

            return ticket;
        }

        public void CreateTicket(TicketDto ticket)
        {
            var newTicket = new Ticket
            {
                ConfirmationNumber = _confirmationNumberUtil.CreateConfirmationNumber(ticket),
                Recipient = ticket.Recipient,
                Email = ticket.Email,
            };

            var performanceId = newTicket.Reservations.First().PerformanceId;
            var capacity = new SeatCapacityDto();
            newTicket.Reservations = _reservationService.GetReservations(capacity, performanceId).ToList();
            
            foreach (var reservation in newTicket.Reservations)
            {
                ticket.TotalPrice += reservation.Price;
                reservation.IsTaken = true;
            }

            _repository.Insert(newTicket);
            _repository.Commit();
        }

        public void EditTicket(TicketDto ticket)
        {
            var ticketToEdit = CheckTicketNullValueById(ticket.Id);

            ticketToEdit.Email = ticket.Email;
            ticketToEdit.Recipient = ticket.Recipient;
            Mapper.Map(ticket.Reservations, ticketToEdit.Reservations);

            _repository.Commit();
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = CheckTicketNullValueById(id);
            _repository.Delete(id);
            _repository.Commit();
        }
    }
}
