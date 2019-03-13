using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Dtos.Venue;
using EventsCalendar.WebUI.ViewModels;

namespace EventsCalendar.WebUI.Controllers
{
    public class PerformancesController : Controller
    {
        private readonly IPerformanceService _performanceService;
        private readonly IPerformerService _performerService;
        private readonly IReservationService _reservationService;
        private readonly IVenueService _venueService;

        public PerformancesController(IPerformanceService performanceService, 
                                      IPerformerService performerService,
                                      IReservationService reservationService,
                                      IVenueService venueService)
        {
            _performanceService = performanceService;
            _performerService = performerService;
            _reservationService = reservationService;
            _venueService = venueService;
        }

        /*
         * GET: Performance
         */
        public ActionResult Index()
        {
            var performanceDtos = _performanceService.GetAllPerformanceDtos();

            ICollection<PerformanceViewModel> viewModels = 
                performanceDtos.Select(performance => new PerformanceViewModel
                {
                    Performance = performance,
                    EventDate = performance.EventDateTime.ToLongDateString(),
                    EventTime = performance.EventDateTime.ToShortTimeString(),
                })
                .ToList();

            return View(viewModels);
        }

        /*
         * Create new performance page
         */
        public ActionResult Create()
        {
            var viewModel = NewPerformanceViewModel();
            return View("PerformanceForm", viewModel);
        }

        /*
         * Edit page
         */
        public ActionResult Edit(int id)
        {
            var viewModel = ReturnPerformanceViewModel(id);
            var prices = _reservationService.GetPrices(id);

            viewModel.Performance.Prices = prices;
            viewModel.Performers = _performerService.GetAllPerformerDtos();
            viewModel.Venues = _venueService.GetAllVenueDtos();
            viewModel.SeatsRemaining = _reservationService.GetSeatsRemaining(id);

            return View("PerformanceForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(PerformanceViewModel performanceViewModel)
        {
            var date = performanceViewModel.EventDate;
            var time = performanceViewModel.EventTime;

            performanceViewModel.Performance.EventDateTime = FixDateTime(date, time);

            if (!ModelState.IsValid)
            {
                performanceViewModel.Performers = _performerService.GetAllPerformerDtos();
                performanceViewModel.Venues = _venueService.GetAllVenueDtos();
                return View("PerformanceForm", performanceViewModel);
            }

            if (performanceViewModel.Performance.Id == 0)
                _performanceService.CreatePerformance(performanceViewModel.Performance);
            else
                _performanceService.EditPerformance(performanceViewModel.Performance);

            return RedirectToAction("Index");
        }

        /*
         * DETAILS: Performance
         */
        public ActionResult Details(int id)
        {
            PerformanceViewModel viewModel = ReturnPerformanceViewModel(id);
            viewModel.SeatsRemaining = _reservationService.GetSeatsRemaining(id);
            var prices = _reservationService.GetPrices(id);
            viewModel.Performance.Prices = prices;
            return View(viewModel);
        }

        private DateTime FixDateTime(string date, string time)
        {
            if (date == null)
                date = DateTime.Now.AddDays(1).ToShortDateString();

            if (time == null)
                time = "7:30 pm";

            DateTime eventDateTime;
            DateTime.TryParseExact($"{date} {time}", new string[] { "M/d/yyyy h:mm tt" },
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                out eventDateTime);

            return eventDateTime;
        }

        /*
         * DELETE: Performance
         */
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _performanceService.DeletePerformance(id);
            return RedirectToAction("Index");
        }

        private PerformanceViewModel NewPerformanceViewModel()
        {
            return new PerformanceViewModel
            {
                Performance = new PerformanceDto
                {
                    PerformerDto = new PerformerDto(),
                    VenueDto = new VenueDto()
                },
                Performers = _performerService.GetAllPerformerDtos(),
                Venues = _venueService.GetAllVenueDtos()
            };
        }

        private PerformanceViewModel ReturnPerformanceViewModel(int id)
        {
            PerformanceViewModel viewModel = new PerformanceViewModel
            {
                Performance = _performanceService.GetPerformanceDtoById(id),
            };

            viewModel.EventTime = viewModel.Performance.EventDateTime.ToShortTimeString();
            viewModel.EventDate = viewModel.Performance.EventDateTime.ToShortDateString();

            return viewModel;
        }

    }
}