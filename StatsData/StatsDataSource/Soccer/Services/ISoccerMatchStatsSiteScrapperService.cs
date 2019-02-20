using System.Collections.Generic;
using StatsDataSource.Soccer.Models;

namespace StatsDataSource.Soccer.Services
{
    public interface ISoccerMatchStatsSiteScrapperService
    {
        IEnumerable<SoccerMatchStatsData> ScrapMatchesStats(IEnumerable<string> matchesStatsUrl);
    }
}