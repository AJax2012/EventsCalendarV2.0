using NUnit.Framework;
using Moq;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Core.Models;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.CrudServices;
using EventsCalendar.Services.Dtos.Performer;

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
        private const string DefaultImgSrc = "https://static1.squarespace.com/static/5ba45d79ab1a620ab25a33da/t/5bf46b1f0e2e72ab66b383f1/1543426766008/Blank+Profile+Pic.png?format=300w";

        private static PerformerDto TestPerformerDto = new PerformerDto
        {
            Id = 1,
            Name = "test",
            Description = "test desc",
            TourName = "test tour",
            IsActive = true,
            ImageUrl = DefaultImgSrc,
            PerformerType = PerformerTypeDto.Musician,
            Genre = GenreDto.Classical
        };

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
            var id = 1;
            _performerRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Performer());
            _target.GetPerformerById(id);
            _performerRepository.Verify(r => r.Find(id), Times.Once());
        }

        [Test]
        public void CreatePerformer_Should_Map_PerformerDto_To_Performer()
        {
            _target.CreatePerformer(TestPerformerDto);

            _performerRepository.Verify(r => r.Insert(It.Is<Performer>(p =>
                p.Name == "test" &&
                p.Description == "test desc" &&
                p.TourName == "test tour" &&
                p.IsActive == true &&
                p.ImageUrl == DefaultImgSrc &&
                p.PerformerTypeId == (int)PerformerTypeDto.Musician &&
                p.GenreId == (int)GenreDto.Classical &&
                p.TopicId == null
            )));
        }
    }
}
