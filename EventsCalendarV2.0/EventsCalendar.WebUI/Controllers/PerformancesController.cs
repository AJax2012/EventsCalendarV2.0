using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Contracts.Services;
using EventsCalendar.Services.Dtos;
using EventsCalendar.WebUI.ViewModels;

namespace EventsCalendar.WebUI.Controllers
{
    public class PerformancesController : Controller
    {
        private readonly IPerformanceService _performanceService;

        public PerformancesController(IPerformanceService performanceService)
        {
            _performanceService = performanceService;
        }

        /*
         * GET: Performance
         */
        public ActionResult Index()
        {
            IEnumerable<IPerformanceViewModel> viewModel = _performanceService.ListPerformances();
            return View(viewModel);
        }

        /*
         * Create new performance page
         */
        public ActionResult Create()
        {
            IPerformanceViewModel viewModel = new PerformanceViewModel
            {
                Performance = new PerformanceDto
                {
                    PerformerDto = new PerformerDto(),
                    VenueDto = new VenueDto()
                },
                Performers = new List<PerformerDto>(),
                Venues = new List<VenueDto>()
            };
            return View("PerformanceForm", _performanceService.NewPerformanceViewModel(viewModel));
        }

        /*
         * Edit page
         */
        public ActionResult Edit(int id)
        {
            IPerformanceViewModel viewModel = new PerformanceViewModel
            {
                Performance = new PerformanceDto
                {
                    Id = id
                },
                Performers = new List<PerformerDto>(),
                Venues = new List<VenueDto>()
            };

            return View("PerformanceForm", _performanceService.ReturnPerformanceViewModel(viewModel));
        }

        [HttpPost]
        public ActionResult Save(PerformanceViewModel performanceViewModel)
        {
            CheckDateTime(performanceViewModel);

            if (!ModelState.IsValid)
            {
                var performanceVm = _performanceService.ReturnPerformanceViewModel
                    (performanceViewModel);

                return View("PerformanceForm", performanceVm);
            }

            if (performanceViewModel.Performance.Id == 0)
                _performanceService.CreatePerformance(performanceViewModel);
            else
                _performanceService.EditPerformance(performanceViewModel, performanceViewModel.Performance.Id);

            return RedirectToAction("Index");
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

        /*
         * DETAILS: Performance
         */
        public ActionResult Details(int id)
        {
            IPerformanceViewModel viewModel = new PerformanceViewModel
            {
                Performance = new PerformanceDto
                {
                    Id = id
                }
            };

            var performance = _performanceService.ReturnPerformanceDetails(viewModel);
            return View(performance);
        }

        /*
         * PRIVATE METHOD
         */
        private void CheckDateTime(IPerformanceViewModel performanceViewModel)
        {
            var date = performanceViewModel.EventDate;
            var time = performanceViewModel.EventTime;

            if (!DateTime.TryParse($"{date} {time}", out var dateValue))
                ModelState.AddModelError("Performance.EventDateTime", "Please insert a valid Date and Time");

            if (DateTime.Compare(dateValue, DateTime.Now) < 0)
                ModelState.AddModelError("Performance.EventDateTime",
                    "Please submit a value later than current date/time");

            performanceViewModel.Performance.EventDateTime = dateValue;
        }
    }
}