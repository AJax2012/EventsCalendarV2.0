using System.Collections.Generic;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Contracts.Services
{
    public interface IPerformanceService
    {
        IEnumerable<PerformanceViewModel> ListPerformances();
        PerformanceViewModel NewPerformanceViewModel();
        void CreatePerformance(PerformanceViewModel performanceViewModel);
        PerformanceViewModel ReturnPerformanceViewModel(int id);
        PerformanceViewModel ReturnPerformanceDetails(int id);
        void EditPerformance(PerformanceViewModel performanceViewModel, int id);
        void DeletePerformance(int id);
    }
}