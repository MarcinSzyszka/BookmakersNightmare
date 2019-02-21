using System;
using System.Linq;
using System.Threading.Tasks;
using DataRepository.Models.Soccer.Stats;
using DataRepository.Services.Soccer.Stats;
using Newtonsoft.Json;
using StatsDataSource.Soccer.Services;

namespace Main.DataManager.Soccer.Stats
{
    public class SoccerStatsDataService : ISoccerStatsDataService
    {
        private readonly ISoccerResultsSiteScraperService _soccerResultsSiteScraperService;
        private readonly ISoccerStatsRepositoryService _soccerStatsRepository;

        public SoccerStatsDataService(ISoccerResultsSiteScraperService soccerResultsSiteScraperService,
            ISoccerStatsRepositoryService soccerStatsRepository)
        {
            _soccerResultsSiteScraperService = soccerResultsSiteScraperService;
            _soccerStatsRepository = soccerStatsRepository;
        }

        public async Task UpdateResultsData(string country, string leagueName, DateTime? afterDate = null)
        {
            var results = await _soccerResultsSiteScraperService.GetLeagueResults(country, leagueName, afterDate);

            var entities = results.Select(r => new SoccerMatchStatsEntity
            {
                Country = country,
                League = leagueName,
                EventDate = r.MatchDate,
                HostsTeam = r.HostsTeam,
                GuestsTeam = r.GuestTeam,
                Result = r.MatchResult,
                StatsJson = JsonConvert.SerializeObject(r.StatsRows)
            }).ToList();

            _soccerStatsRepository.Insert(entities);
        }
    }
}
