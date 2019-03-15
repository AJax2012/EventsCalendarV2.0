using System.Collections.Generic;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Seat;

namespace EventsCalendar.Services.CrudServices
{
    public class PerformanceService : IPerformanceService
    {
        private readonly IRepository<Performance> _repository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IReservationService _reservationService;

        public PerformanceService(IRepository<Performance> repository,
                                  IReservationRepository reservationRepository,
                                  IReservationService reservationService)
        {
            _repository = repository;
            _reservationRepository = reservationRepository;
            _reservationService = reservationService;
        }

        private Performance CheckPerformanceNullValue(int id)
        {
            var performance = _repository.Find(id);

            if (performance == null)
                throw new HttpException(404, "Performance Not Found");

            return performance;
        }

        private Performance MapPerformanceDtoToPerformance(Performance performance, PerformanceDto performanceDto)
        {
            performance.Description = performanceDto.Description;
            performance.IsActive = true;
            performance.EventDateTime = performanceDto.EventDateTime;
            performance.PerformerId = performanceDto.PerformerDto.Id;
            performance.VenueId = performanceDto.VenueDto.Id;
            return performance;
        }

        public void CreatePerformance(PerformanceDto performance)
        {
            var newPerformance = MapPerformanceDtoToPerformance(new Performance(), performance);
            var venueId = performance.VenueDto.Id;
            
            _repository.Insert(newPerformance);
            _repository.Commit();

            var reservations = new SimpleReservationsByType
            {
                BudgetReservations = _reservationService
                    .CreateSimpleReservations(venueId, SeatTypeDto.Budget, performance.Prices.Budget),

                ModerateReservations = _reservationService
                    .CreateSimpleReservations(venueId, SeatTypeDto.Moderate,
                        performance.Prices.Moderate),

                PremierReservations = _reservationService
                    .CreateSimpleReservations(venueId, SeatTypeDto.Premier, performance.Prices.Premier)
            };

            var allReservations = _reservationService.CombineSimpleReservations(reservations);
            _reservationService.InsertReservations(allReservations, newPerformance.Id);
        }

        public IEnumerable<PerformanceDto> GetAllPerformanceDtos()
        {
            return Mapper.Map
                <IEnumerable<Performance>, ICollection<PerformanceDto>>
                (_repository.Collection());
        }

        public IEnumerable<Performance> GetAllPerformances()
        {
            return _repository.Collection();
        }

        public PerformanceDto GetPerformanceDtoById(int performanceId)
        {
            return Mapper.Map
                <Performance, PerformanceDto>
                (CheckPerformanceNullValue(performanceId));
        }

        public Performance GetPerformanceById(int performanceId)
        {
            return CheckPerformanceNullValue(performanceId);
        }

        public void EditPerformance(PerformanceDto performance)
        {
            var performanceToEdit = CheckPerformanceNullValue(performance.Id);
            performanceToEdit = MapPerformanceDtoToPerformance(performanceToEdit, performance);

            var prices = new ReservationPrices
            {
                Budget = performance.Prices.Budget,
                Moderate = performance.Prices.Moderate,
                Premier = performance.Prices.Premier
            };

            _reservationService.SetNewReservationPrices(performanceToEdit.Id, prices);
            _repository.Commit();
        }

        public void DeletePerformance(int performanceId)
        {
            CheckPerformanceNullValue(performanceId);

            // performanceDto.IsActive = false;
            _repository.Delete(performanceId);
            _repository.Commit();
            _reservationRepository.DeleteAllPerformanceReservations(performanceId);
        }
    }
}
