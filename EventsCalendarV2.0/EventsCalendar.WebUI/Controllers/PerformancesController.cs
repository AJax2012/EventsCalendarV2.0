using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
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
            var viewModel = NewIPerformanceViewModel();
            return View("PerformanceForm", viewModel);
        }

        /*
         * Edit page
         */
        public ActionResult Edit(int id)
        {
            var viewModel = NewIPerformanceViewModel();
            viewModel.Performance.Id = id;
            return View("PerformanceForm", _performanceService.ReturnPerformanceViewModel(viewModel));
        }

        [HttpPost]
        public ActionResult Save(PerformanceViewModel performanceViewModel)
        {
            var date = performanceViewModel.EventDate;
            var time = performanceViewModel.EventTime;

            performanceViewModel.Performance.EventDateTime = 
                _performanceService.FixDateTime(date, time);

            if (!ModelState.IsValid)
            {
                var performanceVm = performanceViewModel;
                performanceVm.Performers = _performanceService.GetAllPerformers();
                performanceVm.Venues = _performanceService.GetAllVenues();
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

        private IPerformanceViewModel NewIPerformanceViewModel()
        {
            return new PerformanceViewModel
            {
                Performance = new PerformanceDto
                {
                    PerformerDto = new PerformerDto(),
                    VenueDto = new VenueDto()
                },
                Performers = _performanceService.GetAllPerformers(),
                Venues = _performanceService.GetAllVenues()
            };
        }
    }
}