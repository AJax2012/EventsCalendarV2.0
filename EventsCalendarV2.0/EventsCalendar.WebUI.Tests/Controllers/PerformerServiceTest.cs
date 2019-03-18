using NUnit.Framework;
using Moq;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Core.Models;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.CrudServices;

namespace EventsCalendar.WebUI.Tests.Controllers
{
    /// <summary>
    /// Summary description for PerformerServiceTest
    /// </summary>
    [TestFixture]
    public class PerformerServiceTests
    {
        private Mock<IRepository<Performer>> _performerRepository;
        private Mock<IRepository<Performance>> _performanceRepository;

        private IPerformerService _target;

        [SetUp]
        public void SetUp()
        {
            // This method is called before running the tests
            _performerRepository = new Mock<IRepository<Performer>>();
            _performanceRepository = new Mock<IRepository<Performance>>();
            _target = new PerformerService(
                _performerRepository.Object,
                _performanceRepository.Object);
        }

        [Test]
        public void GetPerformerById_Should_Call_Repository_Find_With_Id()
        {
            // Test id
            var id = 1;

            // This part sets up the mock to return an object, so you don't get the exception
            // The It.IsAny<int>() is because we don't care what value is passed at this point
            _performerRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Performer());

            // Invoke the method we're testing
            _target.GetPerformerById(id);

            // This part verifies that the repository Find method was called with the id we passed in
            _performerRepository.Verify(r => r.Find(id), Times.Once());
        }
    }
}
