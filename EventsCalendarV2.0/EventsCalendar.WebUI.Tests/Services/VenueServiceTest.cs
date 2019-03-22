﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;
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
    }
}