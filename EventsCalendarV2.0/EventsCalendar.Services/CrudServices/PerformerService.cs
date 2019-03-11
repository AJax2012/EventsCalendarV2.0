using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Contracts.Services;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Helpers;

namespace EventsCalendar.Services.CrudServices
{
    public class PerformerService : IPerformerService
    {
        private readonly IRepository<Performer> _repository;
        private readonly IRepository<Performance> _performanceRepository;
        private const string DefaultImgSrc = "https://static1.squarespace.com/static/5ba45d79ab1a620ab25a33da/t/5bf46b1f0e2e72ab66b383f1/1543426766008/Blank+Profile+Pic.png?format=300w";

        public PerformerService(IRepository<Performer> repository,
                                IRepository<Performance> performanceRepository)
        {
            _repository = repository;
            _performanceRepository = performanceRepository;
        }

        private Performer CheckPerformerNullValue(int id)
        {
            Performer performer = _repository.Find(id);
            if (performer == null)
                throw new HttpException(404, "Performer Not Found");

            return performer;
        }

        private PerformerDto MapPerformerToDto(Performer performer)
        {
            var performerDto = new PerformerDto
            {
                Id = performer.Id,
                Name = performer.Name,
                ImageUrl = performer.ImageUrl,
                TourName = performer.TourName,
                IsActive = performer.IsActive,
                PerformerType = performer.PerformerType
            };

            if (performer.PerformerType == PerformerType.Musician)
                performerDto.Genre = performer.Genre;
            else
                performerDto.Topic = performer.Topic;

            return performerDto;
        }

        private IPerformerViewModel mapEnumsToViewModel(IPerformerViewModel viewModel)
        {
            var performerTypes = EnumUtil.GetValues<PerformerType>();
            var genres = EnumUtil.GetValues<Genre>();
            var topics = EnumUtil.GetValues<Topic>();
            Mapper.Map(performerTypes, viewModel.PerformerTypes);
            Mapper.Map(genres, viewModel.Genres);
            Mapper.Map(topics, viewModel.Topics);
            return viewModel;
        }

        public IEnumerable<IPerformerViewModel> ListPerformers()
        {
            IEnumerable<Performer> performers = _repository.Collection().ToList();

            var performerDtos =
                Mapper.Map<IEnumerable<Performer>, IEnumerable<PerformerDto>>
                    (performers);

            var performerViewModels =
                Mapper.Map<IEnumerable<PerformerDto>,
                    IEnumerable<IPerformerViewModel>>(performerDtos);

            return performerViewModels;
        }

        public IPerformerViewModel NewPerformerViewModel(IPerformerViewModel viewModel)
        {
            viewModel.Performer = new PerformerDto();
            viewModel = mapEnumsToViewModel(viewModel);
            viewModel.Performer.ImageUrl = DefaultImgSrc;

            return viewModel;
        }

        public void CreatePerformer(IPerformerViewModel performerViewModel)
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
            _repository.Insert(performer);
            _repository.Commit();
        }

        public IPerformerViewModel ReturnPerformerViewModel(IPerformerViewModel viewModel)
        {
            Performer performer = CheckPerformerNullValue(viewModel.Performer.Id);
            viewModel.Performer = MapPerformerToDto(performer);
            viewModel = mapEnumsToViewModel(viewModel);

            return viewModel;
        }

        public void EditPerformer(IPerformerViewModel performerViewModel, int id)
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

            _repository.Commit();
        }

        public void DeletePerformer(int id)
        {
            CheckPerformerNullValue(id);

            IList<Performance> performances = 
                _performanceRepository.Collection()
                    .Where(p => p.PerformerId == id).ToList();

            // performer.IsActive = false;
            foreach (var performance in performances)
            {
                _performanceRepository.Delete(performance.Id);
                _performanceRepository.Commit();
            }

            _repository.Delete(id);
            _repository.Commit();
        }
    }
}
