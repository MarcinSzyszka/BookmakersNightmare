using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Main.Infrastructure.Enums;
using Newtonsoft.Json;
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
                Country = Country.Gambia,
                League = "gfa-league"
            };

            var result = default(IEnumerable<MatchBet>);

            //Act
            using (_serviceUnderTest)
            {
                result = await _serviceUnderTest.GetMatchBetsWithResultsInLatestSeason(leagueUrl, countryLeague, null);
            }

            //Assert
            Assert.NotNull(result);

            var serializedObject = JsonConvert.SerializeObject(result);

            using (var sw = new StreamWriter(@"oddsResults.json", false))
            {
                await sw.WriteAsync(serializedObject);
            }
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
