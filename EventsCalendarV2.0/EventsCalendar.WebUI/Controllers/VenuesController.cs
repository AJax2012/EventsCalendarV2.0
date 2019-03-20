﻿using System.Linq;
using System.Web.Mvc;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.Services.Dtos.Venue;
using EventsCalendar.Services.Exceptions;
using EventsCalendar.WebUI.ViewModels;

namespace EventsCalendar.WebUI.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IVenueService _venueService;
        private readonly ISeatService _seatService;
        private const string DefaultImgSrc = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJa4VlErDGxyBl-tQu41odZDe-qLvI1xNDALRMYxTITZOb3DslFg";

        public VenuesController(IVenueService venueService, 
                                ISeatService seatService)
        {
            _venueService = venueService;
            _seatService = seatService;
        }

        /*
         * GET: Performance
         */
        public ActionResult Index()
        {
            var venueDtos = _venueService.GetAllVenueDtos();

            var viewModels = venueDtos.Select(venue => 
                new VenueViewModel
                {
                    Venue = _venueService.GetVenueDtoById(venue.Id)
                })
                .ToList();

            return View(viewModels);
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
                    ImageUrl = DefaultImgSrc,
                    SeatCapacity = new SeatCapacityDto()
                },
            };

            return View("VenueForm", viewModel);
        }

        /*
         * Edit Venue page
         */
        public ActionResult Edit(int id)
        {
            try
            {
                VenueViewModel viewModel = new VenueViewModel
                {
                    Venue = _venueService.GetVenueDtoById(id)
                };

                return View("VenueForm", viewModel);
            }
            catch (EntityNotFoundException entity)
            {
                return HttpNotFound(entity.Message);
            }
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
            {
                _venueService.CreateVenue(venueViewModel.Venue);
            }
            else
            {
                try
                {
                    _venueService.EditVenue(venueViewModel.Venue);
                }
                catch (EntityNotFoundException entity)
                {
                    return HttpNotFound(entity.Message);
                }
            }

            return RedirectToAction("Index");
        }

        /*
         * DETAILS: Venue
         */
        public ActionResult Details(int id)
        {
            try
            {
                VenueViewModel viewModel = new VenueViewModel
                {
                    Venue = _venueService.GetVenueDtoById(id)
                };

                return View(viewModel);
            }
            catch (EntityNotFoundException entity)
            {
                return HttpNotFound(entity.Message);
            }
        }

        /*
         * DELETE: Venue
         */
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                _venueService.DeleteVenue(id);
                return RedirectToAction("Index");
            }
            catch (EntityNotFoundException entity)
            {
                return HttpNotFound(entity.Message);
            }
        }
    }
}