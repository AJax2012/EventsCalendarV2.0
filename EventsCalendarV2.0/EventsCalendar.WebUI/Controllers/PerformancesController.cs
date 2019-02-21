using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.ViewModels;

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
            IEnumerable<PerformanceViewModel> viewModel = _performanceService.ListPerformances();
            return View(viewModel);
        }

        /*
         * Create new performance page
         */
        public ActionResult Create()
        {
            return View("PerformanceForm", _performanceService.NewPerformanceViewModel());
        }

        /*
         * Edit page
         */
        public ActionResult Edit(int id)
        {
            return View("PerformanceForm", _performanceService.ReturnPerformanceViewModel(id));
        }

        [HttpPost]
        public ActionResult Save(PerformanceViewModel performanceViewModel)
        {
            CheckDateTime(performanceViewModel);

            if (!ModelState.IsValid)
            {
                var performanceVm = _performanceService.ReturnPerformanceViewModel
                    (performanceViewModel.Performance.Id);

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
            var performance = _performanceService.ReturnPerformanceDetails(id);
            return View(performance);
        }

        /*
         * PRIVATE METHOD
         */
        private void CheckDateTime(PerformanceViewModel performanceViewModel)
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