using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Services.CrudServices
{
    public class PerformanceService : IPerformanceService
    {
        private readonly IRepository<Performance> _context;
        private readonly IRepository<Performer> _performerContext;
        private readonly IGuidRepository<Reservation> _reservationContext;
        private readonly IRepository<Venue> _venueContext;

        public PerformanceService(IRepository<Performance> context,
                                  IRepository<Performer> performerContext, 
                                  IGuidRepository<Reservation> reservationContext,
                                  IRepository<Venue> venueContext)
        {
            _context = context;
            _performerContext = performerContext;
            _reservationContext = reservationContext;
            _venueContext = venueContext;
        }

        private Performance FindPerformanceDto(int id)
        {
            return _context.Find(id);
        }

        private Performance CheckPerformanceNullValue(int id)
        {
            Performance performance = FindPerformanceDto(id);
            if (performance == null)
                throw new HttpException(404, "Performance Not Found");

            return performance;
        }

        public IEnumerable<PerformanceViewModel> ListPerformances()
        {
            IEnumerable<Performance> performances = 
                _context.Collection()
                    .Where(x => x.EventDateTime >= System.DateTime.Today)
                    .ToList();

            var performanceDtos = 
                Mapper.Map<IEnumerable<Performance>, 
                        IEnumerable<PerformanceDto>>
                    (performances);

            var performanceviewModels = 
                Mapper.Map<IEnumerable<PerformanceDto>, 
                    IEnumerable<PerformanceViewModel>>
                    (performanceDtos);

            return performanceviewModels;
        }

        public PerformanceViewModel NewPerformanceViewModel()
        {
            var viewModel = new PerformanceViewModel
            {
                Performance = new PerformanceDto(),
                Performers = Mapper.Map
                    <IEnumerable<Performer>, ICollection<PerformerDto>>
                        (_performerContext.Collection()),

                Venues = Mapper.Map
                    <IEnumerable<Venue>, ICollection<VenueDto>>
                        (_venueContext.Collection())
            };
            
            return viewModel;
        }

        public void CreatePerformance(PerformanceViewModel performanceViewModel)
        {
            var performance = new Performance
            {
                EventDateTime = performanceViewModel.Performance.EventDateTime,
                IsActive = true,
                PerformerId = performanceViewModel.Performance.PerformerDto.Id,
                SeatsRemaining = performanceViewModel.Performance.VenueDto.Capacity,
                VenueId = performanceViewModel.Performance.VenueDto.Id
            };

            foreach (ReservationDto reservationDto in performanceViewModel.Performance.ReservationDtos)
            {
                var reservation = Mapper.Map<ReservationDto, Reservation>(reservationDto);
                try
                {
                    _reservationContext.Commit();
                }
                catch (System.Exception)
                {
                    throw;
                }
                performance.Reservations.Add(reservation);
            }

            _context.Insert(performance);
            _context.Commit();
        }

        public PerformanceViewModel ReturnPerformanceViewModel(int id)
        {
            Performance performance = CheckPerformanceNullValue(id);

            PerformanceViewModel viewModel =
                new PerformanceViewModel
                {
                    Performance = Mapper.Map
                        <Performance, PerformanceDto>(performance),

                    Performers = Mapper.Map
                        <IEnumerable<Performer>, ICollection<PerformerDto>>
                        (_performerContext.Collection()),

                    Venues = Mapper.Map
                        <IEnumerable<Venue>, ICollection<VenueDto>>
                        (_venueContext.Collection()),

                    EventDate = performance.EventDateTime.ToShortDateString(),
                    EventTime = performance.EventDateTime.ToShortTimeString(),
                };

            return viewModel;
        }

        public PerformanceViewModel ReturnPerformanceDetails(int id)
        {
            Performance performance = CheckPerformanceNullValue(id);

            PerformanceViewModel viewModel =
                new PerformanceViewModel
                {
                    Performance = Mapper.Map
                        <Performance, PerformanceDto>(performance),

                    Performers = Mapper.Map
                        <IEnumerable<Performer>, ICollection<PerformerDto>>
                        (_performerContext.Collection()),

                    Venues = Mapper.Map
                        <IEnumerable<Venue>, ICollection<VenueDto>>
                        (_venueContext.Collection()),

                    EventDate = performance.EventDateTime.ToShortDateString(),
                    EventTime = performance.EventDateTime.ToShortTimeString(),
                };

            Mapper.Map(_performerContext.Find(performance.PerformerId), viewModel.Performance.PerformerDto);
            Mapper.Map(_venueContext.Find(performance.VenueId), viewModel.Performance.VenueDto);

            return viewModel;
        }

        public void EditPerformance(PerformanceViewModel performanceViewModel, int id)
        {
            Performance performanceToEdit = CheckPerformanceNullValue(id);

            performanceToEdit.EventDateTime = performanceViewModel.Performance.EventDateTime;
            performanceToEdit.IsActive = true;
            performanceToEdit.PerformerId = performanceViewModel.Performance.PerformerDto.Id;
            performanceToEdit.SeatsRemaining = performanceViewModel.Performance.SeatsRemaining;
            performanceToEdit.VenueId = performanceViewModel.Performance.VenueDto.Id;

            Mapper.Map(performanceViewModel.Performance.ReservationDtos, performanceToEdit.Reservations);

            _context.Commit();
        }

        public void DeletePerformance(int id)
        {
            CheckPerformanceNullValue(id);

//            performance.IsActive = false;
            _context.Delete(id);
            _context.Commit();
        }
    }
}
