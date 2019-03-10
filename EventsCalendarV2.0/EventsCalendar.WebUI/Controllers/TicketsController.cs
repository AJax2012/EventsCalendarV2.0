using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        // Shows User Pre-purchase ticket options page
        public ActionResult Details(Guid id)
        {
            var ticket = _ticketService.ReturnTicketViewModel(id);
            return View(ticket);
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult Create()
        {
            return View();
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult Edit(Guid id)
        {
            return View();
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
    }
}