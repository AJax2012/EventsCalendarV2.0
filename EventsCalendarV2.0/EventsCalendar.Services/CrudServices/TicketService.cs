using AutoMapper;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;
using EventsCalendar.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
        private readonly ISeatService seatService;

        public TicketService(IGuidRepository<Ticket> repository,
                             IReservationRepository reservationRepository,
                             IRepository<Seat> seatRepository,
                             IRepository<Performance> performanceRepository,
                             ConfirmationNumberUtil _confirmationNumberUtil,
                             IReservationService _reservationService,
                             ISeatService _seatService)
        {
            _repository = repository;
            _reservationRepository = reservationRepository;
            _seatRepository = seatRepository;
            _performanceRepository = performanceRepository;
            confirmationNumberUtil = _confirmationNumberUtil;
            reservationService = _reservationService;
            seatService = _seatService;
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
                Email = ticketViewModel.Ticket.Email,
            };

            SeatCapacity capacity = new SeatCapacity();
            Mapper.Map(ticketViewModel, capacity);
            
            IEnumerable<Reservation> reservations = reservationService.CreateReservations(capacity, ticketViewModel.PerformanceId);

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

            TicketViewModel viewModel = new TicketViewModel
            {
                Ticket = Mapper.Map<Ticket, TicketDto>(ticket),

                NumberOfBudget = seatsInTicket
                    .Where(s => s.Seat.SeatType == SeatType.Budget)
                    .Count(),

                NumberOfModerate = seatsInTicket
                    .Where(s => s.Seat.SeatType == SeatType.Moderate)
                    .Count(),

                NumberOfPremier = seatsInTicket
                .Where(s => s.Seat.SeatType == SeatType.Premier)
                .Count()
            };

            return viewModel;
        }

        public TicketViewModel ReturnTicketViewModelByConfirmationNumber(string confirmationNumber)
        {
            Ticket ticket = CheckTicketNullByConfirmationNumber(confirmationNumber);
            ticket.
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
