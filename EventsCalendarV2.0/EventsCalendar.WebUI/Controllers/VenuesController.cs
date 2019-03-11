using System.Collections.Generic;
using System.Web.Mvc;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Contracts.Services;
using EventsCalendar.Services.Dtos;
using EventsCalendar.WebUI.ViewModels;

namespace EventsCalendar.WebUI.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IVenueService _venueService;
        private const string DefaultImgSrc = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJa4VlErDGxyBl-tQu41odZDe-qLvI1xNDALRMYxTITZOb3DslFg";

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        /*
         * GET: Performance
         */
        public ActionResult Index()
        {
            IEnumerable<IVenueViewModel> viewModel = _venueService.ListVenues();
            return View(viewModel);
        }

        /*
         * Create Venue Page
         */
        public ActionResult Create()
        {
            var viewModel = new VenueViewModel
            {
                Venue = new VenueDto
                {
                    Id = 0,
                    AddressId = 0,
                    ImageUrl = DefaultImgSrc
                },
            };

            return View("VenueForm", viewModel);
        }

        /*
         * Edit Venue page
         */
        public ActionResult Edit(int id)
        {
            IVenueViewModel viewModel = new VenueViewModel
            {
                Venue = new VenueDto
                {
                    Id = id
                }
            };

            return View("VenueForm", _venueService.ReturnVenueViewModel(viewModel));
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
            IVenueViewModel viewModel = new VenueViewModel
            {
                Venue = new VenueDto
                {
                    Id = id
                }
            };

            var venue = _venueService.ReturnVenueViewModel(viewModel);

            return View(venue);
        }
    }
}