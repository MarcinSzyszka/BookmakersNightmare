using System;
using System.Threading.Tasks;
using StatsDataSource.Soccer.Services;
using Xunit;

namespace StatsData.Tests.Soccer
{
    public class SoccerResultsSiteScraperServiceIntegrationTests
    {
        [Fact]
        public async Task GetLeagueResults_ShouldReturnAllResultsFromLeaguesSeason()
        {
            //Arrange
            
            //Act
            var result = await _serviceUnderTest.GetLeagueResults("francja", "ligue-1", new DateTime(2019, 2,16));

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

        }
        #region CONFIGURATION

        private ISoccerResultsSiteScraperService _serviceUnderTest;

        public SoccerResultsSiteScraperServiceIntegrationTests()
        {
            _serviceUnderTest = new SoccerResultsSiteScraperService(new SoccerMatchStatsSiteScrapperService());
        }

        #endregion
    }
}
