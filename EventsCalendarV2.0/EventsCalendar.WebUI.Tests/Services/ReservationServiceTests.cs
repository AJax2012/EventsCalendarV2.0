using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.CrudServices;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Seat;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace EventsCalendar.WebUI.Tests.Services
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private Mock<IReservationRepository> _reservationRepository;
        private Mock<ISeatRepository> _seatRepository;
        private IReservationService _target;
        private ISeatService _seatService;
        private Mock<ISeatService> _mockSeatService;

        [SetUp]
        public void SetUp()
        {
            _reservationRepository = new Mock<IReservationRepository>();
            _seatRepository = new Mock<ISeatRepository>();
            _seatService = new SeatService(_seatRepository.Object);
            _mockSeatService = new Mock<ISeatService>();
            _target = new ReservationService(
                _reservationRepository.Object,
                _mockSeatService.Object
            );
        }

        [Test]
        public void CombineSimpleReservations_Should_Map_SimpleReservationsByType_To_IEnumerable()
        {
            var reservation = new SimpleReservation
            {
                SeatId = 1,
                Price = 1
            };

            var reservationList = new List<SimpleReservation>();
            reservationList.Add(reservation);

            var reservationsByType = new SimpleReservationsByType
            {
                BudgetReservations = reservationList,
                ModerateReservations = reservationList,
                PremierReservations = reservationList
            };

            Assert.IsNotEmpty(_target.CombineSimpleReservations(reservationsByType));
        }

        [Test]
        public void CreateSimpleReservations_Should_Return_SimpleReservation_List()
        {
            int venueId = 1;
            var type = SeatTypeDto.Budget;
            decimal price = 1;

            IEnumerable<Seat> seatList = new List<Seat>
            {
                new Seat
                {
                    Id = 1, 
                    VenueId = venueId, 
                    SeatTypeId = 1
                }
            };

            _mockSeatService.Setup(s => s.GetSeatsBySeatType(It.IsAny<int>(), It.IsAny<SeatTypeDto>())).Returns(seatList);
            _target.CreateSimpleReservations(venueId, type, price);
            _mockSeatService.Verify(s => s.GetSeatsBySeatType(It.IsAny<int>(), It.IsAny<SeatTypeDto>()), Times.AtLeastOnce);
        }
    }
}
