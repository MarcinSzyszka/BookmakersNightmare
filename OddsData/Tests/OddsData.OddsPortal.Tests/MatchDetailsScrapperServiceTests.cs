using System;
using System.Linq;
using System.Threading.Tasks;
using OddsData.Infrastructure.Models;
using OddsData.OddsPortal.Services.Scraper;
using OpenQA.Selenium.Chrome;
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
            var result = default(MatchBet);

            //Act
            using (var driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
            {
                result = await _serviceUnderTest.GetMatchBetDetails(driver, matchDetailsUrl);
            }

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Wolves", result.HostsTeamName);
            Assert.Equal("Newcastle", result.GuestsTeamName);
            Assert.Equal(SingleBetResult.Draw, result.FullTime.Result);
            Assert.Equal(SingleBetResult.Draw, result.FirstHalf.Result);
            Assert.Equal(SingleBetResult.Draw, result.SecondHalf.Result);
            Assert.True(result.FullTime.Odds.Average(o => o.Draw) > 0);
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
