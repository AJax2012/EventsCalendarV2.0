using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsCalendar.WebUI.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService ticketService;

        public TicketsController(ITicketService _ticketService)
        {
            ticketService = _ticketService;
        }

        // Shows User Pre-purchase ticket options page
        public ActionResult TicketDetails()
        {
            return View();
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
                ticketService.CreateTicket(ticketViewModel);
            else
            {
                ticketService.EditTicket(ticketViewModel);
            }

            return RedirectToAction("Index", "Venues");
        }
    }
}