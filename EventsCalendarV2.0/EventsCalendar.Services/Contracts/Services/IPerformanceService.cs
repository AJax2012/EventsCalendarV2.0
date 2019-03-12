using System;
using System.Collections.Generic;
using EventsCalendar.Core.Models;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.Contracts.Services
{
    public interface IPerformanceService
    {
        DateTime FixDateTime(string date, string time);
        ICollection<PerformerDto> GetAllPerformers();
        ICollection<VenueDto> GetAllVenues();
        IEnumerable<IPerformanceViewModel> ListPerformances();
        IPerformanceViewModel NewPerformanceViewModel(IPerformanceViewModel viewModel);
        void CreatePerformance(IPerformanceViewModel performanceViewModel);
        IPerformanceViewModel ReturnPerformanceViewModel(IPerformanceViewModel viewModel);
        IPerformanceViewModel ReturnPerformanceDetails(IPerformanceViewModel viewModel);
        void EditPerformance(IPerformanceViewModel performanceViewModel, int id);
        void DeletePerformance(int id);
    }
}