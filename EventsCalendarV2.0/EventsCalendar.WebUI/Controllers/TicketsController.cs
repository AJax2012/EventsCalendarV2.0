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

            if (ticketViewModel.Venue.Id == 0)
                _venueService.CreateVenue(ticketViewModel);
            else
            {
                _venueService.EditVenue(ticketViewModel, ticketViewModel.Venue.Id);
            }

            return RedirectToAction("Index", "Venues");
        }
    }
}