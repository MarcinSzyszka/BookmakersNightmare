using System.Collections.Generic;
using StatsDataSource.Soccer.Services;
using Xunit;

namespace StatsData.Tests.Soccer
{
    public class SoccerMatchStatsSiteScrapperServiceTests
    {
        [Fact]
        public void ScrapMatchesStats_ShouldReturnOneNotEmptyStatsModel()
        {
            //Arrange
            var urls = new List<string>
            {
                @"https://www.flashscore.pl/mecz/zDio29o3/#statystyki-meczu;0"
            };

            //Act
            var result = _serviceUnderTest.ScrapMatchesStats(urls, null);


            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);
        }

        #region CONFIGURATION

        private ISoccerMatchStatsSiteScrapperService _serviceUnderTest;

        public SoccerMatchStatsSiteScrapperServiceTests()
        {
            _serviceUnderTest = new SoccerMatchStatsSiteScrapperService();
        }

        #endregion
    }
}
