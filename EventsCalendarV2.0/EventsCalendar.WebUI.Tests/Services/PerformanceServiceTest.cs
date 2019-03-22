using System;
using System.Collections.Generic;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.CrudServices;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.Services.Dtos.Venue;
using Moq;
using NUnit.Framework;

namespace EventsCalendar.WebUI.Tests.Services
{
    [TestFixture]
    public class PerformanceServiceTest
    {
        private Mock<IRepository<Performance>> _performanceRepository;
        private Mock<IReservationRepository> _reservationRepository;
        private Mock<ISeatRepository> _seatRepository;
        private IPerformanceService _target;
        private IReservationService _reservationService;
        private ISeatService _seatService;

        private const string DefaultPerformerImgSrc = "https://static1.squarespace.com/static/5ba45d79ab1a620ab25a33da/t/5bf46b1f0e2e72ab66b383f1/1543426766008/Blank+Profile+Pic.png?format=300w";
        private const string DefaultVenueImgSrc = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJa4VlErDGxyBl-tQu41odZDe-qLvI1xNDALRMYxTITZOb3DslFg";

        private static readonly ReservationPricesDto TestReservationPricesDto = new ReservationPricesDto
        {
            Budget = 1,
            Moderate = 2,
            Premier = 3
        };

        private static readonly PerformerDto TestPerformerDto = new PerformerDto
        {
            Id = 1,
            Name = "Test Performer",
            Description = "test desc",
            TourName = "test tour",
            IsActive = true,
            ImageUrl = DefaultPerformerImgSrc,
            PerformerType = PerformerTypeDto.Musician,
            Genre = GenreDto.Classical
        };

        private static readonly AddressDto TestAddressDto = new AddressDto
        {
            Id = 1,
            IsActive = true,
            StreetAddress = "123 Main",
            City = "Test City",
            State = "CO",
            ZipCode = "12345"
        };

        private static readonly SeatCapacityDto TestSeatCapaictyDto = new SeatCapacityDto
        {
            Budget = 1,
            Moderate = 2,
            Premier = 3,
            Total = 6
        };

        private static readonly VenueDto TestVenueDto = new VenueDto
        {
            Id = 1,
            Name = "Test Venue",
            ImageUrl = DefaultVenueImgSrc,
            IsActive = true,
            AddressDto = TestAddressDto,
            SeatCapacity = TestSeatCapaictyDto
        };

        private static readonly PerformanceDto TestPerformanceDto = new PerformanceDto
        {
            Id = 1,
            Description = "Test Description",
            IsActive = true,
            Prices = TestReservationPricesDto,
            EventDateTime = DateTime.Today.AddDays(1).AddHours(1),
            PerformerDto = TestPerformerDto,
            VenueDto = TestVenueDto,
            Reservations = new List<ReservationDto>()
        };

        [SetUp]
        public void SetUp()
        {
            _performanceRepository = new Mock<IRepository<Performance>>();
            _reservationRepository = new Mock<IReservationRepository>();
            _seatRepository = new Mock<ISeatRepository>();
            _seatService = new SeatService(_seatRepository.Object);

            _reservationService = new ReservationService(
                _reservationRepository.Object,
                _seatService);

            _target = new PerformanceService(
                _performanceRepository.Object,
                _reservationRepository.Object,
                _reservationService
            );
        }

        [Test]
        public void CreatePerformance_Should_Send_Performer_To_Repository()
        {
            _target.CreatePerformance(TestPerformanceDto);

            _performanceRepository.Verify(r => r.Insert(It.Is<Performance>(p =>
                p.Description == "Test Description" &&
                p.IsActive &&
                p.EventDateTime == DateTime.Today.AddDays(1).AddHours(1) &&
                p.PerformerId == 1 &&
                p.VenueId == 1 &&
                p.Reservations != null
            )));
        }
    }
}
