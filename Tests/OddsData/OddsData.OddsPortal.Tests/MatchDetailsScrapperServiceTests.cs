using System;
using System.Linq;
using System.Threading.Tasks;
using Main.Infrastructure.Enums;
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
                result = await _serviceUnderTest.GetMatchBetDetails(driver, matchDetailsUrl, null);
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

        [Fact]
        public async Task GetMatchBetDetails_ShouldReturnCorrectResults_ForMatchBetween_MarimooAndWellingara()
        {
            //Arrange
            var matchDetailsUrl = @"https://www.oddsportal.com/soccer/gambia/gfa-league/marimoo-wellingara-OYmcOIii/";
            var result = default(MatchBet);

            //Act
            using (var driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
            {
                result = await _serviceUnderTest.GetMatchBetDetails(driver, matchDetailsUrl, null);
            }

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Marimoo", result.HostsTeamName);
            Assert.Equal("Wellingara", result.GuestsTeamName);
            Assert.Equal(SingleBetResult.Draw, result.FullTime.Result);
            Assert.Equal(SingleBetResult.Unknown, result.FirstHalf.Result);
            Assert.Equal(SingleBetResult.Unknown, result.SecondHalf.Result);
            Assert.True(result.FullTime.Odds.Average(o => o.Draw) > 0);
        }

        [Fact]
        public async Task GetMatchBetDetails_ShouldReturnCorrectResults_ForMatchBetween_HawksAndGambiaPorts()
        {
            //Arrange
            var matchDetailsUrl = @"https://www.oddsportal.com/soccer/gambia/gfa-league/hawks-gambia-ports-EsqZYZyM/";
            var result = default(MatchBet);

            //Act
            using (var driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
            {
                result = await _serviceUnderTest.GetMatchBetDetails(driver, matchDetailsUrl, null);
            }

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Hawks", result.HostsTeamName);
            Assert.Equal("Gambia Ports", result.GuestsTeamName);
            Assert.Equal(SingleBetResult.Draw, result.FullTime.Result);
            Assert.Equal(SingleBetResult.Draw, result.FirstHalf.Result);
            Assert.Equal(SingleBetResult.Unknown, result.SecondHalf.Result);
            Assert.True(result.FullTime.Odds.Average(o => o.Draw) > 0);
            Assert.True(result.FirstHalf.Odds.Average(o => o.Draw) > 0);
            Assert.Empty(result.SecondHalf.Odds);
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
