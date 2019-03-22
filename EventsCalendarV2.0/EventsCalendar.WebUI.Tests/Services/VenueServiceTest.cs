using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.CrudServices;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.Services.Dtos.Venue;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace EventsCalendar.WebUI.Tests.Services
{
    [TestFixture]
    public class VenueServiceTest
    {
        private Mock<IRepository<Venue>> _venueRepository;
        private Mock<IRepository<Address>> _addressRepository;
        private Mock<ISeatRepository> _seatRepository;
        private Mock<IRepository<Performance>> _performanceRepository;
        private IVenueService _target;
        private ISeatService _seatService;
        private const string DefaultImgSrc = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJa4VlErDGxyBl-tQu41odZDe-qLvI1xNDALRMYxTITZOb3DslFg";

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
            Name = "Test",
            ImageUrl = DefaultImgSrc,
            IsActive = true,
            AddressDto = TestAddressDto,
            SeatCapacity = TestSeatCapaictyDto
        };

        public VenueServiceTest()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
            });
        }

        [SetUp]
        public void SetUp()
        {
            _venueRepository = new Mock<IRepository<Venue>>();
            _addressRepository = new Mock<IRepository<Address>>();
            _seatRepository = new Mock<ISeatRepository>();
            _performanceRepository = new Mock<IRepository<Performance>>();
            _seatService = new SeatService(_seatRepository.Object);
            _target = new VenueService(
                _venueRepository.Object,
                _addressRepository.Object,
                _seatRepository.Object,
                _performanceRepository.Object,
                _seatService
            );
        }

        [Test]
        public void CreateVenue_Should_Send_Venue_To_Repository()
        {
            _target.CreateVenue(TestVenueDto);

            _venueRepository.Verify(r => r.Insert(It.Is<Venue>(v =>
                v.Name == "Test" &&
                v.ImageUrl == DefaultImgSrc &&
                v.IsActive
            )));
        }

        [Test]
        public void GetVenueDtoById_Should_Return_VenueDto_From_Id()
        {
            var id = 1;
            _venueRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Venue());
            _target.GetVenueDtoById(id);
            _venueRepository.Verify(r => r.Find(id), Times.Once);
        }

        [Test]
        public void GetVenueById_Should_Return_Venue_From_Id()
        {
            var id = 1;
            _venueRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Venue());
            _target.GetVenueById(id);
            _venueRepository.Verify(r => r.Find(id), Times.Once);
        }

        [Test]
        public void GetAllVenueDtos_Should_Return_VenueDto_Collection()
        {
            _venueRepository.Setup(r => r.Collection()).Returns(new List<Venue>());
            _target.GetAllVenueDtos();
            _venueRepository.Verify(r => r.Collection(), Times.AtLeastOnce);
        }

        [Test]
        public void GetAllVenueDtos_Should_Return_Null()
        {
            _venueRepository.Setup(r => r.Collection()).Returns(null as List<Venue>);
            Assert.IsEmpty(_target.GetAllVenueDtos());
        }

        [Test]
        public void GetAllVenues_Should_Return_Venue_Collection()
        {
            _venueRepository.Setup(r => r.Collection()).Returns(new List<Venue>());
            _target.GetAllVenues();
            _venueRepository.Verify(r => r.Collection(), Times.AtLeastOnce);
        }

        [Test]
        public void GetAllVenues_Should_Return_Null()
        {
            _venueRepository.Setup(r => r.Collection()).Returns(null as List<Venue>);
            Assert.IsNull(_target.GetAllVenues());
        }
    }
}
