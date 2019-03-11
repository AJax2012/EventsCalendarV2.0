using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Contracts.Services;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.CrudServices
{
    public class PerformanceService : IPerformanceService
    {
        private readonly IRepository<Performance> _repository;
        private readonly IRepository<Performer> _performerRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IRepository<Venue> _venueRepository;
        private readonly IReservationService _reservationService;

        public PerformanceService(IRepository<Performance> repository,
                                  IRepository<Performer> performerRepository, 
                                  IReservationRepository reservationRepository,
                                  IRepository<Venue> venueRepository,
                                  IReservationService reservationService)
        {
            _repository = repository;
            _performerRepository = performerRepository;
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

        private SeatCapacityDto CountRemainingReservations(Performance performance)
        {
            return _reservationService.GetSeatsRemaining(performance.Id);
        }

        public IEnumerable<IPerformanceViewModel> ListPerformances()
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
                    IEnumerable<IPerformanceViewModel>>
                    (performanceDtos);

            return performanceViewModels;
        }

        public IPerformanceViewModel NewPerformanceViewModel(IPerformanceViewModel viewModel)
        {
            Mapper.Map(_performerRepository.Collection(), viewModel.Performers);
            Mapper.Map(_venueRepository.Collection(), viewModel.Venues);
            
            return viewModel;
        }

        public void CreatePerformance(IPerformanceViewModel performanceViewModel)
        {
            var performerId = performanceViewModel.Performance.PerformerDto.Id;
            var venueId = performanceViewModel.Performance.VenueDto.Id;
            var performance = new Performance
            {
                Description = performanceViewModel.Performance.Description,
                EventDateTime = performanceViewModel.Performance.EventDateTime,
                IsActive = true,
                PerformerId = performanceViewModel.Performance.PerformerDto.Id,
                VenueId = performanceViewModel.Performance.VenueDto.Id,
                Performer = _performerRepository.Find(performerId),
                Venue = _venueRepository.Find(venueId),
                Reservations = new List<Reservation>()
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

        public IPerformanceViewModel ReturnPerformanceViewModel(IPerformanceViewModel viewModel)
        {
            Performance performance = CheckPerformanceNullValue(viewModel.Performance.Id);

            var performers = _performerRepository.Collection();
            var venues = _venueRepository.Collection();

            Mapper.Map(performance, viewModel.Performance);
            Mapper.Map(performers, viewModel.Performers);
            Mapper.Map(venues, viewModel.Venues);
            Mapper.Map(viewModel.Performance, viewModel);

            var prices = _reservationRepository.GetPrices(performance.Id);
            viewModel.BudgetPrice = prices.Budget;
            viewModel.ModeratePrice = prices.Moderate;
            viewModel.PremierPrice = prices.Premier;

            return viewModel;
        }

        public IPerformanceViewModel ReturnPerformanceDetails(IPerformanceViewModel viewModel)
        {
            Performance performance = CheckPerformanceNullValue(viewModel.Performance.Id);

            Mapper.Map(performance, viewModel.Performance);
            Mapper.Map(_performerRepository.Collection(), viewModel.Performers);
            Mapper.Map(_venueRepository.Collection(), viewModel.Venues);
            viewModel.EventDate = performance.EventDateTime.ToShortDateString();
            viewModel.EventTime = performance.EventDateTime.ToShortTimeString();
            viewModel.BudgetPrice = _reservationRepository.GetPrices(performance.Id).Budget;
            viewModel.ModeratePrice = _reservationRepository.GetPrices(performance.Id).Moderate;
            viewModel.PremierPrice = _reservationRepository.GetPrices(performance.Id).Premier;
            viewModel.SeatsRemaining = CountRemainingReservations(performance);

            Mapper.Map(_performerRepository.Find(performance.PerformerId), viewModel.Performance.PerformerDto);
            Mapper.Map(_venueRepository.Find(performance.VenueId), viewModel.Performance.VenueDto);

            return viewModel;
        }

        public void EditPerformance(IPerformanceViewModel performanceViewModel, int id)
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
            _repository.Delete(id);
            _repository.Commit();
            _reservationRepository.DeleteAllPerformanceReservations(id);
        }
    }
}
