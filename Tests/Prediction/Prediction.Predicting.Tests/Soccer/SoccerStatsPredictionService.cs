using System.Collections.Generic;
using Autofac;
using Prediction.Predicting.Soccer.Services;
using Prediction.Predicting.Tests.Fixtures;
using Xunit;

namespace Prediction.Predicting.Tests.Soccer
{
    public class SoccerStatsPredictionService : IClassFixture<MainFixture>
    {
        [Fact]
        public void PredictResultsDependsOnAllResults_ShouldReturnPredictionResults()
        {
            //Arrange
            var teamNamesToPredict = new List<string>
            {
                "Villarreal",
                "Sporting",
                "Frankfurt",
                "Szachtar"
            };

            //Act
            var result = _serviceUnderTest.PredictResultsDependsOnAllResults(teamNamesToPredict, 3);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }

        #region CONFIGURATION

        private ISoccerStatsPredictionService _serviceUnderTest;

        public SoccerStatsPredictionService(MainFixture fixture)
        {
            _serviceUnderTest = fixture.Container.Resolve<ISoccerStatsPredictionService>();
        }

        #endregion
    }
}
