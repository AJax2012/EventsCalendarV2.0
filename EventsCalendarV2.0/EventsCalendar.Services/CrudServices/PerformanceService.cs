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
        private readonly IReservationService _reservationService;

        public PerformanceService(IRepository<Performance> repository,
                                  IRepository<Performer> performerRepository, 
                                  IReservationRepository reservationRepository,
                                  ISeatRepository seatRepository,
                                  IRepository<Venue> venueRepository,
                                  IReservationService reservationService)
        {
            _repository = repository;
            _performerRepository = performerRepository;
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
            _venueRepository = venueRepository;
            _reservationService = reservationService;
        }

        private Performance CheckPerformanceNullValue(int id)
        {
            var performance = _repository.Find(id);
            if (performance == null)
                throw new HttpException(404, "Performance Not Found");

            return performance;
        }

        private SeatCapacity CountRemainingReservations(Performance performance)
        {
            var capacity = new SeatCapacity();
            return _reservationService.GetSeatsRemaining(performance.Id);
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

            var performanceViewModels = 
                Mapper.Map<IEnumerable<PerformanceDto>, 
                    IEnumerable<PerformanceViewModel>>
                    (performanceDtos);

            return performanceViewModels;
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
                VenueId = performanceViewModel.Performance.VenueDto.Id,
            };

            IEnumerable<SimpleReservation> budgetReservations = _reservationService.CreateSimpleReservations(performance.VenueId, SeatType.Budget, performanceViewModel.BudgetPrice);
            IEnumerable<SimpleReservation> moderateReservations = _reservationService.CreateSimpleReservations(performance.VenueId, SeatType.Moderate, performanceViewModel.ModeratePrice);
            IEnumerable<SimpleReservation> premierReservations = _reservationService.CreateSimpleReservations(performance.VenueId, SeatType.Premier, performanceViewModel.PremierPrice);

            var reservations = new SimpleReservationsByType
            {
                BudgetReservations = budgetReservations,
                ModerateReservations = moderateReservations,
                PremierReservations = premierReservations
            };

            IEnumerable<SimpleReservation> allReservations = _reservationService.CombineSimpleReservations(reservations);
                        
            _repository.Insert(performance);
            _repository.Commit();

            _reservationRepository.BulkInsertReservations(allReservations, performance.Id);
        }

        public PerformanceViewModel ReturnPerformanceViewModel(int id)
        {
            Performance performance = CheckPerformanceNullValue(id);

            var viewModel = new PerformanceViewModel
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
                SeatsRemaining = CountRemainingReservations(performance),
                BudgetPrice = _reservationRepository.GetPrices(id).Budget,
                ModeratePrice = _reservationRepository.GetPrices(id).Moderate,
                PremierPrice = _reservationRepository.GetPrices(id).Premier,
            };

            return viewModel;
        }

        public PerformanceViewModel ReturnPerformanceDetails(int id)
        {
            Performance performance = CheckPerformanceNullValue(id);

            var viewModel = new PerformanceViewModel
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
                BudgetPrice = _reservationRepository.GetPrices(id).Budget,
                ModeratePrice = _reservationRepository.GetPrices(id).Moderate,
                PremierPrice = _reservationRepository.GetPrices(id).Premier,
                SeatsRemaining = CountRemainingReservations(performance),
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
            performanceToEdit.VenueId = performanceViewModel.Performance.VenueDto.Id;

            var prices = new ReservationPrices
            {
                Budget = performanceViewModel.BudgetPrice,
                Moderate = performanceViewModel.ModeratePrice,
                Premier = performanceViewModel.PremierPrice
            };

            _reservationService.SetNewReservationPrices(id, prices);

            _repository.Commit();
        }

        public void DeletePerformance(int id)
        {
            CheckPerformanceNullValue(id);

            // performance.IsActive = false;
            _reservationRepository.DeleteAllPerformanceReservations(id);
            _repository.Delete(id);
            _repository.Commit();
        }
    }
}
