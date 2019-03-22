using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.CrudServices;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.Services.Dtos.Venue;
using EventsCalendar.Services.Exceptions;
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

        public PerformanceServiceTest()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
            });
        }

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

        [Test]
        public void EditPerformance_Should_Update_Repository_Object()
        {
            _performanceRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Performance());
            _target.EditPerformance(TestPerformanceDto);

            _performanceRepository.Verify(r => r.Update(It.Is<Performance>(p =>
                p.Description == "Test Description" &&
                p.IsActive &&
                p.EventDateTime == DateTime.Today.AddDays(1).AddHours(1) &&
                p.PerformerId == 1 &&
                p.VenueId == 1 &&
                p.Reservations != null
            )));
        }

        [Test]
        public void GetAllPerformanceDtos_Should_Return_All_Performances_In_Repository()
        {
            _performanceRepository.Setup(r => r.Collection()).Returns(new List<Performance>());
            _target.GetAllPerformanceDtos();
            _performanceRepository.Verify(r => r.Collection(), Times.AtLeastOnce);
        }

        [Test]
        public void GetAllPerformanceDtos_Should_Return_Empty_When_No_Performances()
        {
            _performanceRepository.Setup(r => r.Collection()).Returns(null as List<Performance>);
            Assert.IsEmpty(_target.GetAllPerformanceDtos());
        }

        [Test]
        public void GetAllPerformances_Should_Return_All_Performances_In_Repository()
        {
            _performanceRepository.Setup(r => r.Collection()).Returns(new List<Performance>());
            _target.GetAllPerformances();
            _performanceRepository.Verify(r => r.Collection(), Times.AtLeastOnce);
        }

        [Test]
        public void GetAllPerformances_Should_Return_Return_Null_When_No_Performances()
        {
            _performanceRepository.Setup(r => r.Collection()).Returns(null as List<Performance>);
            Assert.IsNull(_target.GetAllPerformances());
        }

        [Test]
        public void GetPerformanceDtoById_Should_Call_Repository_Find_With_Id()
        {
            var id = 1;
            _performanceRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Performance());
            _target.GetPerformanceById(id);
            _performanceRepository.Verify(r => r.Find(id), Times.Once);
        }

        [Test]
        public void GetPerformanceDtoById_When_Id_Not_Found_Should_Throw_Exception()
        {
            int id = 1;
            _performanceRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(null as Performance);
            Assert.Throws<EntityNotFoundException>(() => _target.GetPerformanceById(id));
        }

        [Test]
        public void GetPerformanceById_Should_Call_Repository_Find_With_Id()
        {
            var id = 1;
            _performanceRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Performance());
            _target.GetPerformanceById(id);
            _performanceRepository.Verify(r => r.Find(id), Times.Once);
        }

        [Test]
        public void GetPerformanceById_When_Id_Not_Found_Should_Throw_Exception()
        {
            int id = 1;
            _performanceRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(null as Performance);
            Assert.Throws<EntityNotFoundException>(() => _target.GetPerformanceById(id));
        }

        [Test]
        public void DeletePerformance_Should_Call_Repository_With_Delete_With_Id()
        {
            var id = 1;
            var performance = new Performance {Id = id};
            performance.Reservations.Add(new Reservation{Id = new Guid()});
            
            _performanceRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(performance);
            _target.DeletePerformance(id);

            _performanceRepository.Verify(r => r.Delete(id));
            _reservationRepository.Verify(r => r.DeleteAllPerformanceReservations(id));
        }

        [Test]
        public void DeletePerformance_When_Id_Not_Found_Should_Throw_Exception()
        {
            int id = 1;
            _performanceRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(null as Performance);
            Assert.Throws<EntityNotFoundException>(() => _target.DeletePerformance(id));
        }
    }
}
