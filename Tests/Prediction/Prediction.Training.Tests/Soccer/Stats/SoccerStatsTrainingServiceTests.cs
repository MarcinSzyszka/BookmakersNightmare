using System;
using System.IO;
using Prediction.Training.Soccer.Stats;
using Xunit;

namespace Prediction.Training.Tests.Soccer.Odds
{
    public class SoccerStatsTrainingServiceTests
    {
        [Fact]
        public void Train_ShouldTrainModelCorrectly()
        {
            //Arrange
            var trainingSetFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Soccer", "Stats", "trainingSet.csv");

            //Act
            _serviceUnderTest.Train(trainingSetFilePath);

            //Assert

        }

        #region CONFIGURATION

        private ISoccerStatsTrainingService _serviceUnderTest;

        public SoccerStatsTrainingServiceTests()
        {
            _serviceUnderTest = new SoccerStatsTrainingService();
        }

        #endregion
    }
}
