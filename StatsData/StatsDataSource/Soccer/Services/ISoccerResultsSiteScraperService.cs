using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StatsDataSource.Soccer.Models;

namespace StatsDataSource.Soccer.Services
{
    public interface ISoccerResultsSiteScraperService
    {
        Task<IEnumerable<SoccerMatchStatsData>> GetLeagueResults(string country, string league, DateTime? afterDate);
    }
}