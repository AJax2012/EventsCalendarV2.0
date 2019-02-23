using System.Collections.Generic;
using System.Web.Mvc;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.ViewModels;
using EventsCalendar.Services;

namespace EventsCalendar.WebUI.Controllers
{
    public class PerformersController : Controller
    {
        private readonly IPerformerService _performerService;

        public PerformersController(IPerformerService performerService)
        {
            _performerService = performerService;
        }

        /*
         * GET: Performance
         */
        public ActionResult Index()
        {
            IEnumerable<PerformerViewModel> viewModel = _performerService.ListPerformers();
            return View(viewModel);
        }

        /*
         * Create new performerViewModel page
         */
        public ActionResult Create(/*HttpPostedFileBase image*/)
        {
            return View("PerformerForm", _performerService.NewPerformerViewModel());
        }
        
        /*
         * Edit Page
         */
        public ActionResult Edit(int id)
        {
            return View("PerformerForm", _performerService.ReturnPerformerViewModel(id));
        }

        [HttpPost]
        public ActionResult Save(PerformerViewModel performerViewModel)
        {
            if (!ModelState.IsValid)
                return View("PerformerForm", performerViewModel);

            if (performerViewModel.Performer.Id == 0)
                _performerService.CreatePerformer(performerViewModel);
            else
                _performerService.EditPerformer(performerViewModel, performerViewModel.Performer.Id);

            return RedirectToAction("Index", "Performers");
        }

        /*
         * DELETE: Performer
         */
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _performerService.DeletePerformer(id);
            return RedirectToAction("Index");
        }

        /*
         * DETAILS: Performer
         */
        public ActionResult Details(int id)
        {
            var performer = _performerService.ReturnPerformerViewModel(id);

            return View(performer);
        }
    }
}