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
        private readonly IRepository<Performance> _repository;
        private readonly IRepository<Performer> _performerRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IRepository<Venue> _venueRepository;

        public PerformanceService(IRepository<Performance> repository,
                                  IRepository<Performer> performerRepository, 
                                  IReservationRepository reservationRepository,
                                  ISeatRepository seatRepository,
                                  IRepository<Venue> venueRepository)
        {
            _repository = repository;
            _performerRepository = performerRepository;
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
            _venueRepository = venueRepository;
        }

        private Performance FindPerformanceDto(int id)
        {
            return _repository.Find(id);
        }

        private Performance CheckPerformanceNullValue(int id)
        {
            Performance performance = FindPerformanceDto(id);
            if (performance == null)
                throw new HttpException(404, "Performance Not Found");

            return performance;
        }

        private IEnumerable<Seat> GetSeatsBySeatType(int id, SeatType type)
        {
            return _seatRepository.Collection()
                .Where(seat => seat.VenueId == id)
                .Where(seat => seat.SeatType == type)
                .ToList();
        }

        private IEnumerable<SimpleReservation> GetSimpleReservations(int venueId, SeatType type, decimal price)
        {
            IEnumerable<Seat> seats = GetSeatsBySeatType(venueId, type);
            List<SimpleReservation> reservations = new List<SimpleReservation>();

            foreach (var seat in seats)
            {
                reservations.Add(new SimpleReservation
                {
                    SeatId = seat.Id,
                    Price = price
                });
            }

            return reservations;
        }

        private IEnumerable<SimpleReservation> CombineReservations(IEnumerable<SimpleReservation> list1, IEnumerable<SimpleReservation> list2, IEnumerable<SimpleReservation> list3)
        {
            List<SimpleReservation> all = new List<SimpleReservation>();
            all.AddRange(list1);
            all.AddRange(list2);
            all.AddRange(list3);
            return all;
        }

        public IEnumerable<PerformanceViewModel> ListPerformances()
        {
            IEnumerable<Performance> performances = 
                _repository.Collection()
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
                        (_performerRepository.Collection()),

                Venues = Mapper.Map
                    <IEnumerable<Venue>, ICollection<VenueDto>>
                        (_venueRepository.Collection())
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
                VenueId = performanceViewModel.Performance.VenueDto.Id
            };

            IEnumerable<SimpleReservation> budgetReservations = GetSimpleReservations(performance.VenueId, SeatType.Budget, performanceViewModel.BudgetPrice);
            IEnumerable<SimpleReservation> moderateReservations = GetSimpleReservations(performance.VenueId, SeatType.Moderate, performanceViewModel.ModeratePrice);
            IEnumerable<SimpleReservation> premierReservations = GetSimpleReservations(performance.VenueId, SeatType.Premier, performanceViewModel.PremierPrice);

            IEnumerable<SimpleReservation> allReservations = CombineReservations(budgetReservations, moderateReservations, premierReservations);


            _repository.Insert(performance);
            _repository.Commit();

            _reservationRepository.BulkInsertReservations(allReservations, performance.Id);
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
                        (_performerRepository.Collection()),

                    Venues = Mapper.Map
                        <IEnumerable<Venue>, ICollection<VenueDto>>
                        (_venueRepository.Collection()),

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
                        (_performerRepository.Collection()),

                    Venues = Mapper.Map
                        <IEnumerable<Venue>, ICollection<VenueDto>>
                        (_venueRepository.Collection()),

                    EventDate = performance.EventDateTime.ToShortDateString(),
                    EventTime = performance.EventDateTime.ToShortTimeString(),
                };

            Mapper.Map(_performerRepository.Find(performance.PerformerId), viewModel.Performance.PerformerDto);
            Mapper.Map(_venueRepository.Find(performance.VenueId), viewModel.Performance.VenueDto);

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

            _repository.Commit();
        }

        public void DeletePerformance(int id)
        {
            CheckPerformanceNullValue(id);

//            performance.IsActive = false;
            _repository.Delete(id);
            _repository.Commit();
        }
    }
}
