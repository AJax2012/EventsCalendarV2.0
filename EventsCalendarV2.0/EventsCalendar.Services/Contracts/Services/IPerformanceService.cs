using System.Collections.Generic;

namespace EventsCalendar.Services.Contracts.Services
{
    public interface IPerformanceService
    {
        IEnumerable<IPerformanceViewModel> ListPerformances();
        IPerformanceViewModel NewPerformanceViewModel(IPerformanceViewModel viewModel);
        void CreatePerformance(IPerformanceViewModel performanceViewModel);
        IPerformanceViewModel ReturnPerformanceViewModel(IPerformanceViewModel viewModel);
        IPerformanceViewModel ReturnPerformanceDetails(IPerformanceViewModel viewModel);
        void EditPerformance(IPerformanceViewModel performanceViewModel, int id);
        void DeletePerformance(int id);
    }
}