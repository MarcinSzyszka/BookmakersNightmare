using System.Threading.Tasks;
using OddsData.Infrastructure.Models;
using OddsData.OddsPortal.Services.Scraper;
using Xunit;

namespace OddsData.OddsPortal.Tests
{
    public class MatchDetailsScrapperServiceTests
    {
        [Fact]
        public async Task GetMatchBetDetails_ShouldReturnCorrectResults_ForMatchBetween_WolvesAndNewcastle()
        {
            //Arrange
            var matchDetailsUrl = @"https://www.oddsportal.com/soccer/england/premier-league/wolves-newcastle-utd-nNNqedbR/";

            //Act
            var result = await _serviceUnderTest.GetMatchBetDetails(matchDetailsUrl);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Wolves", result.HostsTeamName);
            Assert.Equal("Newcastle", result.GuestsTeamName);
            Assert.Equal(SingleBetResult.Draw, result.FullTime.Result);
            Assert.Equal(SingleBetResult.Draw, result.FirstHalf.Result);
            Assert.Equal(SingleBetResult.Draw, result.SecondHalf.Result);
            Assert.True(result.FullTime.OddsAverage.Draw > 0);
        }



        #region CONFIGURATION

        private IMatchDetailsScraperService _serviceUnderTest;

        public MatchDetailsScrapperServiceTests()
        {
            this._serviceUnderTest = new MatchDetailsScraperService();
        }

        #endregion
    }
}
