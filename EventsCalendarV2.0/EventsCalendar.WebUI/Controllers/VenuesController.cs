using System.Collections.Generic;
using System.Web.Mvc;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.WebUI.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        /*
         * GET: Performance
         */
        public ActionResult Index()
        {
            IEnumerable<VenueViewModel> viewModel = _venueService.ListVenues();
            return View(viewModel);
        }

        /*
         * Create Venue Page
         */
        public ActionResult Create()
        {
            return View("VenueForm", _venueService.NewVenueViewModel());
        }

        /*
         * Edit Venue page
         */
        public ActionResult Edit(int id)
        {
            return View("VenueForm", _venueService.ReturnVenueViewModel(id));
        }

        /*
         * Save Venues Form
         */
        [HttpPost]
        public ActionResult Save(VenueViewModel venueViewModel)
        {
            if (!ModelState.IsValid)
                return View("VenueForm", venueViewModel);

            if (venueViewModel.Venue.Id == 0)
                _venueService.CreateVenue(venueViewModel);
            else
            {
                _venueService.EditVenue(venueViewModel, venueViewModel.Venue.Id);
            }

            return RedirectToAction("Index", "Venues");
        }

        /*
         * DELETE: Venue
         */
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _venueService.DeleteVenue(id);
            return RedirectToAction("Index");
        }

        /*
         * DETAILS: Venue
         */
        public ActionResult Details(int id)
        {
            var venue = _venueService.ReturnVenueViewModel(id);

            return View(venue);
        }
    }
}