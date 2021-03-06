﻿using System.IO;
using Autofac;
using Prediction.LearningData.Soccer.Services;
using Prediction.LearningData.Tests.Fixtures;
using Xunit;

namespace Prediction.LearningData.Tests.Soccer
{
    public class SoccerStatsLearningDataServiceTests : IClassFixture<MainFixture>
    {
        [Fact]
        public void PrepareStatsLearningDataAndGetResultsFilePath_ShouldReturnPathToNotEmptyFile()
        {
            //Arrange

            //Act
            var result = _serviceUnderTest.PrepareLearningDataAndGetResultsFilePath(3);

            //Assert
            Assert.NotNull(result);
            Assert.True(File.Exists(result));
        }

        [Fact]
        public void PrepareStatsLearningDataAndGetResultsFilePath_PrepareDataForTeam_ShouldReturnPathToNotEmptyFile()
        {
            //Arrange

            //Act
            var result = _serviceUnderTest.PrepareLearningDataAndGetResultsFilePath("Villarreal", 3);

            //Assert
            Assert.NotNull(result);
            Assert.True(File.Exists(result));
        }

        #region CONFIGURATION

        private ISoccerStatsLearningDataService _serviceUnderTest;
        private MainFixture _fixture;

        public SoccerStatsLearningDataServiceTests(MainFixture fixture)
        {
            _fixture = fixture;
            _serviceUnderTest = fixture.Container.Resolve<ISoccerStatsLearningDataService>();
        }

        #endregion
    }
}
