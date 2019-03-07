using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;
using EventsCalendar.Services.Helpers;

namespace EventsCalendar.Services.CrudServices
{
    public class PerformerService : IPerformerService
    {
        private readonly IRepository<Performer> _context;
        private readonly IRepository<CustomImage> _imageContext;
        private readonly IRepository<Performance> _performanceContext;
        private readonly string DefaultImgSrc = "https://static1.squarespace.com/static/5ba45d79ab1a620ab25a33da/t/5bf46b1f0e2e72ab66b383f1/1543426766008/Blank+Profile+Pic.png?format=300w";

        public PerformerService(IRepository<Performer> context,
                                IRepository<CustomImage> imageContext,
                                IRepository<Performance> performanceContext)
        {
            _context = context;
            _imageContext = imageContext;
            _performanceContext = performanceContext;
        }

        private Performer FindPerformerDto(int id)
        {
            return _context.Find(id);
        }

        private Performer CheckPerformerNullValue(int id)
        {
            Performer performer = FindPerformerDto(id);
            if (performer == null)
                throw new HttpException(404, "Performer Not Found");

            return performer;
        }

        private PerformerDto MapPerformerToDto(Performer performer)
        {
            PerformerDto performerDto = new PerformerDto();
            performerDto.Id = performer.Id;
            performerDto.Name = performer.Name;
            performerDto.ImageUrl = performer.ImageUrl;
            performerDto.TourName = performer.TourName;
            performerDto.IsActive = performer.IsActive;
            performerDto.PerformerType = performer.PerformerType;
            if (performer.PerformerType == PerformerType.Musician)
                performerDto.Genre = performer.Genre;
            else
                performerDto.Topic = performer.Topic;

            return performerDto;
        }

        public IEnumerable<PerformerViewModel> ListPerformers()
        {
            IEnumerable<Performer> performers = _context.Collection().ToList();

            var performerDtos =
                Mapper.Map<IEnumerable<Performer>, IEnumerable<PerformerDto>>
                    (performers);

            var performerViewModels =
                Mapper.Map<IEnumerable<PerformerDto>,
                    IEnumerable<PerformerViewModel>>(performerDtos);

            return performerViewModels;
        }

        public PerformerViewModel NewPerformerViewModel()
        {
            var viewModel = new PerformerViewModel
            {
                Performer = new PerformerDto(),
            };

            viewModel.PerformerTypes = EnumUtil.GetValues<PerformerType>();
            viewModel.Genres = EnumUtil.GetValues<Genre>();
            viewModel.Topics = EnumUtil.GetValues<Topic>();

            return viewModel;
        }

        public void CreatePerformer(PerformerViewModel performerViewModel)
        {
            var performer = new Performer
            {
                Description = performerViewModel.Performer.Description,
                IsActive = true,
                Name = performerViewModel.Performer.Name,
                PerformerType = performerViewModel.Performer.PerformerType,
                ImageUrl = performerViewModel.Performer.ImageUrl,
                TourName = performerViewModel.Performer.TourName
            };

            if (string.IsNullOrWhiteSpace(performer.ImageUrl))
                performer.ImageUrl = DefaultImgSrc;
            
            if (performer.PerformerType.Equals(PerformerType.Musician))
                performer.Genre = performerViewModel.Performer.Genre;
            else
                performer.Topic = performerViewModel.Performer.Topic;

            performer.IsActive = true;
            _context.Insert(performer);
            _context.Commit();
        }

        public PerformerViewModel ReturnPerformerViewModel(int id)
        {
            Performer performer = CheckPerformerNullValue(id);

            var viewModel = new PerformerViewModel
            {
                PerformerTypes = EnumUtil.GetValues<PerformerType>(),
                Genres = EnumUtil.GetValues<Genre>(),
                Topics = EnumUtil.GetValues<Topic>()
            };

            viewModel.Performer = MapPerformerToDto(performer);

            return viewModel;
        }

        public void EditPerformer(PerformerViewModel performerViewModel, int id)
        {
            Performer performerToEdit = CheckPerformerNullValue(id);

            performerToEdit.Description = performerViewModel.Performer.Description;
            performerToEdit.IsActive = true;
            performerToEdit.Name = performerViewModel.Performer.Name;
            performerToEdit.PerformerType = performerViewModel.Performer.PerformerType;
            performerToEdit.TourName = performerViewModel.Performer.TourName;
            performerToEdit.ImageUrl = performerViewModel.Performer.ImageUrl;

            if (performerToEdit.PerformerType.Equals(PerformerType.Musician))
                performerToEdit.Genre = performerViewModel.Performer.Genre;
            else
                performerToEdit.Topic = performerViewModel.Performer.Topic;

            _context.Commit();
        }

        public void DeletePerformer(int id)
        {
            CheckPerformerNullValue(id);

            IList<Performance> performances = 
                _performanceContext.Collection()
                    .Where(p => p.PerformerId == id).ToList();

            // performer.IsActive = false;
            foreach (var performance in performances)
            {
                _performanceContext.Delete(performance.Id);
                _performanceContext.Commit();
            }

            _context.Delete(id);
            _context.Commit();
        }
    }
}
