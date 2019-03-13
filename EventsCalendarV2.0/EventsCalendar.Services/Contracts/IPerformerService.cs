using System.Collections.Generic;
using EventsCalendar.Core.Models;
using EventsCalendar.Services.Dtos.Performer;

namespace EventsCalendar.Services.Contracts
{
    public interface IPerformerService
    {
        IEnumerable<PerformerTypeDto> GetPerformerTypeValues();
        IEnumerable<GenreDto> GetGenreValues();
        IEnumerable<TopicDto> GetTopicValues();
        void CreatePerformer(PerformerDto performer);
        ICollection<PerformerDto> GetAllPerformerDtos();
        IEnumerable<Performer> GetAllPerformers();
        PerformerDto GetPerformerDtoById(int id);
        Performer GetPerformerById(int id);
        void EditPerformer(PerformerDto performer);
        void DeletePerformer(int id);
    }
}