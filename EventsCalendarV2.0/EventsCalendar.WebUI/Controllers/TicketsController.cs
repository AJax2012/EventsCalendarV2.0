using System;
using System.Linq;
using System.Web.Mvc;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.WebUI.ViewModels;

namespace EventsCalendar.WebUI.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IReservationService _reservationService;

        public TicketsController(ITicketService ticketService, IReservationService reservationService)
        {
            this._ticketService = ticketService;
            _reservationService = reservationService;
        }

        public ActionResult Index()
        {
            var tickets = _ticketService.GetAllTicketDtos();
            var viewModel = tickets.Select(ticket => new TicketViewModel {Ticket = ticket}).ToList();
            return View(viewModel);
        }

        public ActionResult SearchTickets()
        {
            return View();
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult DetailsFromId(Guid id)
        {
            var viewModel = ReturnTicketViewModelWithTicket(id);
            return View("Details", viewModel);
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult DetailsFromConfirmationNumber(string confirmationNumber)
        {
            var ticket = _ticketService.GetTicketDtoByConfirmationNumber(confirmationNumber);
            var viewModel = ReturnTicketViewModelWithTicket(ticket.Id);
            return View("Details", viewModel);
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult Create(int id)
        {
            var viewModel = new TicketViewModel
            {
                PerformanceId = id,
                Ticket = new TicketDto(),
                SeatCapacity = new SeatCapacityDto()
            };

            viewModel.Ticket.Id = Guid.Empty;

            return View("TicketForm", viewModel);
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult Edit(Guid id)
        {
            return View("TicketForm", ReturnTicketViewModelWithTicket(id));
        }

        /*
         * Save Ticket Form
         */
        [HttpPost]
        public ActionResult Save(TicketViewModel ticketViewModel)
        {
            if (!ModelState.IsValid)
                return View("TicketForm", ticketViewModel);

            ticketViewModel.Ticket.Reservations = _reservationService
                .GetReservationDtos(ticketViewModel.SeatCapacity, ticketViewModel.PerformanceId);

            if (ticketViewModel.Ticket.Id == Guid.Empty)
                _ticketService.CreateTicket(ticketViewModel.Ticket);
            else
            {
                _ticketService.EditTicket(ticketViewModel.Ticket);
            }

            return RedirectToAction("Index", "Venues");
        }

        public ActionResult Delete(Guid id)
        {
            _ticketService.DeleteTicket(id);
            return RedirectToAction("Index", "Performances");
        }

        private TicketViewModel ReturnTicketViewModelWithTicket(Guid id)
        {
            var viewModel = new TicketViewModel
            {
                Ticket = _ticketService.GetTicketDtoById(id),
            };

            viewModel.BudgetPrice =viewModel.Ticket.Reservations
                .Single(r => r.SeatId == (int) SeatTypeDto.Budget).Price;

            viewModel.ModeratePrice = viewModel.Ticket.Reservations
                .Single(r => r.SeatId == (int) SeatTypeDto.Moderate).Price;

            viewModel.PremierPrice = viewModel.Ticket.Reservations
                .Single(r => r.SeatId == (int) SeatTypeDto.Premier).Price;

            viewModel.PerformanceId = viewModel.Ticket.Reservations.Single().PerformanceID;
            viewModel.SeatCapacity.Budget = viewModel.SeatCapacity.Budget;
            viewModel.SeatCapacity.Moderate = viewModel.SeatCapacity.Moderate;
            viewModel.SeatCapacity.Premier = viewModel.SeatCapacity.Premier;

            return viewModel;
        }
    }
}