using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OddsData.Infrastructure.Models;

namespace OddsData.OddsPortal.Services.Scraper
{
    public interface IWebScraperService : IDisposable
    {
        Task<IEnumerable<MatchBet>> GetMatchBetsWithResultsInLatestSeason(string baseUrl, CountryLeague countryLeague);
    }
}