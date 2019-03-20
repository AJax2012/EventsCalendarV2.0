using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Exceptions;
using EventsCalendar.WebUI.ViewModels;

namespace EventsCalendar.WebUI.Controllers
{
    public class PerformersController : Controller
    {
        private readonly IPerformerService _performerService;
        private const string DefaultImgSrc = "https://static1.squarespace.com/static/5ba45d79ab1a620ab25a33da/t/5bf46b1f0e2e72ab66b383f1/1543426766008/Blank+Profile+Pic.png?format=300w";

        public PerformersController(IPerformerService performerService)
        {
            _performerService = performerService;
        }

        /*
         * GET: Performance
         */
        public ActionResult Index()
        {
            var allPerformerDtos = _performerService.GetAllPerformerDtos();

            ICollection<PerformerViewModel> viewModel = 
                allPerformerDtos.Select(performer => 
                    new PerformerViewModel
                    {
                        Performer = performer
                    }).ToList();

            return View(viewModel);
        }

        /*
         * Create new performerViewModel page
         */
        public ActionResult Create()
        {
            PerformerViewModel viewModel = NewPerformerViewModel();
            return View("PerformerForm", viewModel);
        }
        
        /*
         * Edit Page
         */
        [HttpPost]
        public ActionResult Save(PerformerViewModel performerViewModel)
        {
            if (!ModelState.IsValid)
            {
                performerViewModel.PerformerTypes = _performerService.GetPerformerTypeValues();
                performerViewModel.Genres = _performerService.GetGenreValues();
                performerViewModel.Topics = _performerService.GetTopicValues();
                return View("PerformerForm", performerViewModel);
            }

            if (performerViewModel.Performer.Id == 0)
                _performerService.CreatePerformer(performerViewModel.Performer);
            else
            {
                try
                {
                    _performerService.EditPerformer(performerViewModel.Performer);
                }
                catch (EntityNotFoundException entity)
                {
                    return new HttpNotFoundResult(entity.Message);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View("PerformerForm", ReturnPerformerViewModel(id));
            }
            catch (EntityNotFoundException entity)
            {
                return new HttpNotFoundResult(entity.Message);
            }
        }

        /*
         * DELETE: Performer
         */
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                _performerService.DeletePerformer(id);
                return RedirectToAction("Index");
            }
            catch (EntityNotFoundException entity)
            {
                return HttpNotFound(entity.Message);
            }
        }

        /*
         * DETAILS: Performer
         */
        public ActionResult Details(int id)
        {
            try
            {
                var viewModel = new PerformerViewModel
                {
                    Performer = _performerService.GetPerformerDtoById(id),
                    PerformerTypes = _performerService.GetPerformerTypeValues(),
                    Genres = _performerService.GetGenreValues(),
                    Topics = _performerService.GetTopicValues()
                };

                return View(viewModel);
            }
            catch (EntityNotFoundException entity)
            {
                return new HttpNotFoundResult(entity.Message);
            }
        }

        private PerformerViewModel NewPerformerViewModel()
        {
            return new PerformerViewModel
            {
                Performer = new PerformerDto
                {
                    ImageUrl = DefaultImgSrc
                },
                PerformerTypes = _performerService.GetPerformerTypeValues(),
                Genres = _performerService.GetGenreValues(),
                Topics = _performerService.GetTopicValues()
            };
        }

        private PerformerViewModel ReturnPerformerViewModel(int performerId)
        {
            return new PerformerViewModel
            {
                Performer = _performerService.GetPerformerDtoById(performerId),
                PerformerTypes = _performerService.GetPerformerTypeValues(),
                Genres = _performerService.GetGenreValues(),
                Topics = _performerService.GetTopicValues()
            };
        }
    }
}