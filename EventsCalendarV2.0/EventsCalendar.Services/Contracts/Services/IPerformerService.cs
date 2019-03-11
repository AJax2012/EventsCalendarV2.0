using System.Collections.Generic;

namespace EventsCalendar.Services.Contracts.Services
{
    public interface IPerformerService
    {
        IEnumerable<IPerformerViewModel> ListPerformers();
        IPerformerViewModel NewPerformerViewModel(IPerformerViewModel viewModel);
        void CreatePerformer(IPerformerViewModel viewModel);
        IPerformerViewModel ReturnPerformerViewModel(IPerformerViewModel viewModel);
        void EditPerformer(IPerformerViewModel viewModel, int id);
        void DeletePerformer(int id);
    }
}