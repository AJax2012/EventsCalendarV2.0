using System.Collections.Generic;
using EventsCalendar.Core.Models;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Performer;

namespace EventsCalendar.Services.Contracts
{
    public interface IPerformanceService
    {
        void CreatePerformance(PerformanceDto performance);
        void EditPerformance(PerformanceDto performance);
        IEnumerable<PerformanceDto> GetAllPerformanceDtos();
        IEnumerable<Performance> GetAllPerformances();
        PerformanceDto GetPerformanceDtoById(int performanceId);
        Performance GetPerformanceById(int performanceId);
        void DeletePerformance(int performanceId);
    }
}