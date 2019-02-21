using System;
using System.IO;
using Prediction.Training.Soccer.Odds;
using Xunit;

namespace Prediction.Training.Tests.Soccer.Odds
{
    public class SoccerOddsTrainingServiceTests
    {
        [Fact]
        public void Train_ShouldTrainModelCorrectly()
        {
            //Arrange
            var trainingSetFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Soccer", "Odds", "trainingSet.csv");

            //Act
            _serviceUnderTest.Train(trainingSetFilePath);

            //Assert

        }

        #region CONFIGURATION

        private ISoccerOddsTrainingService _serviceUnderTest;

        public SoccerOddsTrainingServiceTests()
        {
            _serviceUnderTest = new SoccerOddsTrainingService();
        }

        #endregion
    }
}
