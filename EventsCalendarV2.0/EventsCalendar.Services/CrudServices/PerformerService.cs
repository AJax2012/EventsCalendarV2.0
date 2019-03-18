using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos.Performer;
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
            var performer = _repository.Find(id);
            if (performer == null)
                throw new HttpException(404, "Performer Not Found");

            return performer;
        }

        private Performer MapPerformerDtoToPerformerModel(Performer performer, PerformerDto performerDto)
        {
            performer.Name = performerDto.Name;
            performer.Description = performerDto.Description;
            performer.TourName = performerDto.TourName;
            performer.IsActive = true;
            performer.ImageUrl = performerDto.ImageUrl;
            performer.PerformerTypeId = (int)performerDto.PerformerType;

            if (performer.PerformerTypeId == (int) PerformerTypeDto.Musician)
                performer.GenreId = (int) performerDto.Genre;
            else
                performer.TopicId = (int) performerDto.Topic;

            return performer;
        }

        public IEnumerable<PerformerTypeDto> GetPerformerTypeValues()
        {
            return EnumUtil.GetValues<PerformerTypeDto>();
        }

        public IEnumerable<GenreDto> GetGenreValues()
        {
            return EnumUtil.GetValues<GenreDto>();
        }

        public IEnumerable<TopicDto> GetTopicValues()
        {
            return EnumUtil.GetValues<TopicDto>();
        }

        public void CreatePerformer(PerformerDto performer)
        {
            var newPerformer = MapPerformerDtoToPerformerModel(new Performer(), performer);
            _repository.Insert(newPerformer);
            _repository.Commit();
        }

        public ICollection<PerformerDto> GetAllPerformerDtos()
        {
            return Mapper.Map
                <ICollection<Performer>, ICollection<PerformerDto>>
                (_repository.Collection().ToList());
        }

        public IEnumerable<Performer> GetAllPerformers()
        {
            return _repository.Collection();
        }

        public PerformerDto GetPerformerDtoById(int id)
        {
            return Mapper.Map<Performer, PerformerDto>(CheckPerformerNullValue(id));
        }

        public Performer GetPerformerById(int id)
        {
            return CheckPerformerNullValue(id);
        }

        public void EditPerformer(PerformerDto performerDto)
        {
            Performer performer = CheckPerformerNullValue(performerDto.Id);
            performer = MapPerformerDtoToPerformerModel(performer, performerDto);
            _repository.Update(performer);
        }

        public void DeletePerformer(int id)
        {
            var performer = CheckPerformerNullValue(id);

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
