using System.IO;
using Autofac;
using Prediction.LearningData.Soccer.Services.Odds;
using Prediction.LearningData.Tests.Fixtures;
using Xunit;

namespace Prediction.LearningData.Tests.Soccer.Odds
{
    public class SoccerOddsLearningDataServiceTests : IClassFixture<MainFixture>
    {
        [Fact]
        public void PrepareFullTimeDataAndGetResultsFilePath_ShouldReturnPathToNotEmptyFile()
        {
            //Arrange
            //var fullTimeSoccerRepository = _fixture.Container.Resolve<ISoccerRepositoryService<SoccerFullTimeMatchBetEntity>>();

            //fullTimeSoccerRepository.Insert(new SoccerFullTimeMatchBetEntity
            //{
            //    HostsTeam = "Blablabla"
            //});

            //Act
            var result = _serviceUnderTest.PrepareFullTimeDataAndGetResultsFilePath();

            //Assert
            Assert.NotNull(result);
            Assert.True(File.Exists(result));
        }

        #region CONFIGURATION

        private ISoccerOddsLearningDataService _serviceUnderTest;
        private MainFixture _fixture;

        public SoccerOddsLearningDataServiceTests(MainFixture fixture)
        {
            _fixture = fixture;
            _serviceUnderTest = fixture.Container.Resolve<ISoccerOddsLearningDataService>();
        }

        #endregion
    }
}
