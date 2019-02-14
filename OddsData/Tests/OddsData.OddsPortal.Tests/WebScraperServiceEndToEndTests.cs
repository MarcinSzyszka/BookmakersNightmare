using System.Threading.Tasks;
using Moq;
using OddsData.Infrastructure.Enums;
using OddsData.Infrastructure.Models;
using OddsData.OddsPortal.Services.Scraper;
using Xunit;

namespace OddsData.OddsPortal.Tests
{
    public class WebScraperServiceEndToEndTests
    {
        [Fact]
        public async Task GetMatchBetsWithResultsInLatestSeason_IntegrationTests_ShouldReturnResultsFromCyprusFirstDivision()
        {
            //Arrange
            var leagueUrl = @"https://www.oddsportal.com";
            var countryLeague = new CountryLeague
            {
                Country = Country.Qatar,
                League = "Division-2"
            };

            //Act
            var result = await _serviceUnderTest.GetMatchBetsWithResultsInLatestSeason(leagueUrl, countryLeague);

            //Assert
            Assert.NotNull(result);
        }

        #region CONFIGURATION

        readonly IWebScraperService _serviceUnderTest;

        public WebScraperServiceEndToEndTests()
        {
            _serviceUnderTest = new WebScraperService(new MatchDetailsScraperService());
        }

        #endregion
    }
}
