using EventsCalendar.Core.ViewModels;
using System;
using System.Web.Mvc;
using EventsCalendar.Core.Contracts.Services;

namespace EventsCalendar.WebUI.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            this._ticketService = ticketService;
        }

        public ActionResult Index()
        {
            var tickets = _ticketService.ListTickets();
            return View(tickets);
        }

        public ActionResult SearchTickets()
        {
            return View();
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult DetailsFromId(Guid id)
        {
            var ticket = _ticketService.ReturnTicketViewModelById(id);

            return View("Details", ticket);
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult DetailsFromConfirmationNumber(string confirmationNumber)
        {
            var ticket = _ticketService.ReturnTicketViewModelByConfirmationNumber(confirmationNumber);

            return View("Details", ticket);
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult Create(int performanceId)
        {
            return View("TicketForm", _ticketService.NewTicketViewModel(performanceId));
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult Edit(Guid id)
        {
            return View("TicketForm", _ticketService.ReturnTicketViewModelById(id));
        }

        /*
         * Save Ticket Form
         */
        [HttpPost]
        public ActionResult Save(TicketViewModel ticketViewModel)
        {
            if (!ModelState.IsValid)
                return View("TicketForm", ticketViewModel);

            if (ticketViewModel.Ticket.Id == Guid.Empty)
                _ticketService.CreateTicket(ticketViewModel);
            else
            {
                _ticketService.EditTicket(ticketViewModel);
            }

            return RedirectToAction("Index", "Venues");
        }

        public ActionResult Delete(Guid id)
        {
            _ticketService.DeleteTicket(id);
            return RedirectToAction("Index", "Performances");
        }
    }
}