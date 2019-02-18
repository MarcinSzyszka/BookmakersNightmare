using System;
using System.IO;
using Prediction.Training.Soccer;
using Xunit;

namespace Prediction.Training.Tests.Soccer
{
    public class SoccerTrainingServiceTests
    {
        [Fact]
        public void Train_ShouldTrainModelCorrectly()
        {
            //Arrange
            var trainingSetFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Soccer", "trainingSet.csv");

            //Act
            _serviceUnderTest.Train(trainingSetFilePath);

            //Assert

        }

        #region CONFIGURATION

        private ISoccerTrainingService _serviceUnderTest;

        public SoccerTrainingServiceTests()
        {
            _serviceUnderTest = new SoccerTrainingService();
        }

        #endregion
    }
}
