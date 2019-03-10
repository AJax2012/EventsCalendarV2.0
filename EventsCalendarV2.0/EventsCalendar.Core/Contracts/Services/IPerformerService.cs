using System.Collections.Generic;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Contracts.Services
{
    public interface IPerformerService
    {
        IEnumerable<PerformerViewModel> ListPerformers();
        PerformerViewModel NewPerformerViewModel();
        void CreatePerformer(PerformerViewModel performerViewModel);
        PerformerViewModel ReturnPerformerViewModel(int id);
        void EditPerformer(PerformerViewModel performerViewModel, int id);
        void DeletePerformer(int id);
    }
}