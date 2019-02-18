using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Main.Infrastructure.Enums;
using OddsData.Infrastructure.Models;
using OddsData.Infrastructure.Services;
using OddsData.OddsPortal.Services.Scraper;

namespace OddsData.OddsPortal.Services
{
    public class OddsPortalOddsDataService : IOddsDataService
    {
        private readonly IWebScraperService _webScraper;

        public OddsPortalOddsDataService(IWebScraperService webScraper)
        {
            _webScraper = webScraper;
        }

        public Task<IEnumerable<MatchBet>> GetResults(Country country, string leagueName, DateTime? fromDate)
        {
            var url = ConfigurationManager.AppSettings["OddsPortalUrl"];

            var countryLeague = new CountryLeague
            {
                Country = country,
                League = leagueName
            };

            return _webScraper.GetMatchBetsWithResultsInLatestSeason(url, countryLeague, fromDate);
        }
    }
}
