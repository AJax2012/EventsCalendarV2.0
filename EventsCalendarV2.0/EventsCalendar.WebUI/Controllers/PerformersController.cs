using System.Collections.Generic;
using System.Web.Mvc;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Contracts.Services;
using EventsCalendar.Services.Dtos;
using EventsCalendar.WebUI.ViewModels;

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
            IEnumerable<IPerformerViewModel> viewModel = _performerService.ListPerformers();
            return View(viewModel);
        }

        /*
         * Create new performerViewModel page
         */
        public ActionResult Create()
        {
            IPerformerViewModel viewModel = new PerformerViewModel();
            return View("PerformerForm", _performerService.NewPerformerViewModel(viewModel));
        }
        
        /*
         * Edit Page
         */
        public ActionResult Edit(int id)
        {
            IPerformerViewModel viewModel = new PerformerViewModel
            {
                Performer = new PerformerDto
                {
                    Id = id
                }
            };

            return View("PerformerForm", _performerService.ReturnPerformerViewModel(viewModel));
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
            IPerformerViewModel viewModel = new PerformerViewModel
            {
                Performer = new PerformerDto
                {
                    Id = id
                }
            };

            return View(_performerService.ReturnPerformerViewModel(viewModel));
        }
    }
}