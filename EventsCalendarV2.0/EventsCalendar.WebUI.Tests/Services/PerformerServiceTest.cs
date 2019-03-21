using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.CrudServices;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Exceptions;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EventsCalendar.WebUI.Tests.Services
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
        private const string DefaultImgSrc = "https://static1.squarespace.com/static/5ba45d79ab1a620ab25a33da/t/5bf46b1f0e2e72ab66b383f1/1543426766008/Blank+Profile+Pic.png?format=300w";

        private static readonly PerformerDto TestPerformerDto = new PerformerDto
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

        public PerformerServiceTests()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
            });
        }


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
        public void MapPerformerDtoToPerformerModel_Should_Map_DTO_to_Performer()
        {
            var expected = new Performer
            {
                Name = "test",
                Description = "test desc",
                TourName = "test tour",
                IsActive = true,
                ImageUrl = DefaultImgSrc,
                PerformerTypeId = (int) PerformerTypeDto.Musician,
                GenreId = (int) GenreDto.Classical
            };

            var performer =  _target.MapPerformerDtoToPerformerModel(new Performer(), TestPerformerDto);
            performer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetPerformerTypeValues_Should_Return_PerformerDto_Values()
        {
            var expected = new [] { 
                PerformerTypeDto.Musician, 
                PerformerTypeDto.PublicSpeaker
            };

            var actual = _target.GetPerformerTypeValues();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetGenreValues_Should_Return_GenreDto_Values()
        {
            var expected = new [] { 
                GenreDto.Alternative,
                GenreDto.Blues,
                GenreDto.Classical,
                GenreDto.ClassicRock,
                GenreDto.Jazz,
                GenreDto.Rock
            };

            var actual = _target.GetGenreValues();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetTopicValues_Should_Return_TopicDto_Values()
        {
            var expected = new [] { 
                TopicDto.Ecology,
                TopicDto.Economics,
                TopicDto.Politics,
                TopicDto.Racism
            };

            var actual = _target.GetTopicValues();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreatePerformer_Should_Map_PerformerDto_To_Performer()
        {
            _target.CreatePerformer(TestPerformerDto);

            _performerRepository.Verify(r => r.Insert(It.Is<Performer>(p =>
                p.Name == "test" &&
                p.Description == "test desc" &&
                p.TourName == "test tour" &&
                p.IsActive &&
                p.ImageUrl == DefaultImgSrc &&
                p.PerformerTypeId == (int)PerformerTypeDto.Musician &&
                p.GenreId == (int)GenreDto.Classical &&
                p.TopicId == null
            )));
        }

        [Test]
        public void GetAllPerformerDtos_Should_Return_All_Performers_In_Repository_And_Map_To_Dto()
        {
            _performerRepository.Setup(r => r.Collection()).Returns(new List<Performer>());
            _target.GetAllPerformerDtos();
            _performerRepository.Verify(r => r.Collection(), Times.AtLeastOnce);
        }

        [Test]
        public void GetAllPerformers_Should_Return_All_Performers_In_Repository()
        {
            
            _performerRepository.Setup(r => r.Collection()).Returns(new List<Performer>());
            _target.GetAllPerformers();
            _performerRepository.Verify(r => r.Collection(), Times.AtLeastOnce);
        }

        [Test]
        public void GetAllPerformers_Should_Return_Empty_When_No_Performers()
        {
            _performerRepository.Setup(r => r.Collection()).Returns(null as List<Performer>);
            Assert.IsNull(_target.GetAllPerformers());
        }

        [Test]
        public void GetPerformerDtoById_Should_Call_Repository_Find_With_Id()
        {
            var id = 1;
            _performerRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Performer());
            _target.GetPerformerDtoById(id);
            _performerRepository.Verify(r => r.Find(id), Times.Once());
        }

        [Test]
        public void GetPerformerDtoById_When_Id_Not_Found_Should_Throw_Exception()
        {
            int id = 1;
            _performerRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(null as Performer);
            Assert.Throws<EntityNotFoundException>(() => _target.GetPerformerDtoById(id));
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
        public void GetPerformerById_When_Id_Not_Found_Should_Throw_Exception()
        {
            int id = 1;
            _performerRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(null as Performer);
            Assert.Throws<EntityNotFoundException>(() => _target.GetPerformerById(id));
        }

        [Test]
        public void EditPerformer_When_Performer_Not_Found_Should_Throw_Exception()
        {
            _performerRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(null as Performer);
            Assert.Throws<EntityNotFoundException>(() => _target.EditPerformer(TestPerformerDto));
        }

        [Test]
        public void EditPerformer_Should_Map_PerformerDto_To_Existing_Performer()
        {
            _performerRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(new Performer());
            _target.EditPerformer(TestPerformerDto);

            _performerRepository.Verify(r => r.Update(It.Is<Performer>(p => 
                p.Name == "test" &&
                p.Description == "test desc" &&
                p.TourName == "test tour" &&
                p.IsActive &&
                p.ImageUrl == DefaultImgSrc &&
                p.PerformerTypeId == (int)PerformerTypeDto.Musician &&
                p.GenreId == (int)GenreDto.Classical &&
                p.TopicId == null
            )));
        }

        [Test]
        public void DeletePerformer_Should_Call_Repository_With_Delete_With_Id()
        {
            int id = 1;
            var performer = new Performer{Id = id};
            performer.Performances.Add(new Performance{Id = id});

            _performerRepository.Setup(r => r.Find(It.IsAny<int>())).Returns(performer);
            _target.DeletePerformer(id);

            _performerRepository.Verify(r => r.Delete(id));
            _performanceRepository.Verify(r => r.Delete(id));
        }
    }
}
