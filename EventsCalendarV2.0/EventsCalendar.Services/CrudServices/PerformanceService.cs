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
        private readonly ISeatService seatService;
        private readonly IReservationService reservationService;

        public PerformanceService(IRepository<Performance> repository,
                                  IRepository<Performer> performerRepository, 
                                  IReservationRepository reservationRepository,
                                  ISeatRepository seatRepository,
                                  IRepository<Venue> venueRepository,
                                  ISeatService _seatService,
                                  IReservationService _reservationService)
        {
            _repository = repository;
            _performerRepository = performerRepository;
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
            _venueRepository = venueRepository;
            seatService = _seatService;
            reservationService = _reservationService;
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

        private SeatCapacity CountRemainingReservations(Performance performance)
        {
            SeatCapacity capacity = new SeatCapacity();
            capacity.Budget = performance.BudgetSeatsRemaining;
            capacity.Moderate = performance.ModerateSeatsRemaining;
            capacity.Premier = performance.PremierSeatsRemaining;
            return capacity;
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
                VenueId = performanceViewModel.Performance.VenueDto.Id,
            };

            IEnumerable<SimpleReservation> budgetReservations = reservationService.CreateSimpleReservations(performance.VenueId, SeatType.Budget, performanceViewModel.BudgetPrice);
            IEnumerable<SimpleReservation> moderateReservations = reservationService.CreateSimpleReservations(performance.VenueId, SeatType.Moderate, performanceViewModel.ModeratePrice);
            IEnumerable<SimpleReservation> premierReservations = reservationService.CreateSimpleReservations(performance.VenueId, SeatType.Premier, performanceViewModel.PremierPrice);

            IEnumerable<SimpleReservation> allReservations = reservationService.CombineReservations(budgetReservations, moderateReservations, premierReservations);

            performance.BudgetSeatsRemaining = budgetReservations.Count();
            performance.ModerateSeatsRemaining = moderateReservations.Count();
            performance.PremierSeatsRemaining = premierReservations.Count();

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

            viewModel.SeatsRemaining = CountRemainingReservations(performance);
            viewModel.BudgetPrice = _reservationRepository.GetPrices(id).Budget;
            viewModel.ModeratePrice = _reservationRepository.GetPrices(id).Moderate;
            viewModel.PremierPrice = _reservationRepository.GetPrices(id).Premier;

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
                    BudgetPrice = _reservationRepository.GetPrices(id).Budget,
                    ModeratePrice = _reservationRepository.GetPrices(id).Moderate,
                    PremierPrice = _reservationRepository.GetPrices(id).Premier,
                };

            viewModel.SeatsRemaining = CountRemainingReservations(performance);

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
            performanceToEdit.BudgetSeatsRemaining = performanceViewModel.SeatsRemaining.Budget;
            performanceToEdit.ModerateSeatsRemaining = performanceViewModel.SeatsRemaining.Moderate;
            performanceToEdit.PremierSeatsRemaining = performanceViewModel.SeatsRemaining.Premier;
            performanceToEdit.VenueId = performanceViewModel.Performance.VenueDto.Id;

            ReservationPrices prices = new ReservationPrices
            {
                Budget = performanceViewModel.BudgetPrice,
                Moderate = performanceViewModel.ModeratePrice,
                Premier = performanceViewModel.PremierPrice
            };

            reservationService.SetNewReservationPrices(id, prices);

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
