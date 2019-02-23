using System.Collections.Generic;
using System.Linq;
using DataRepository.Services.Soccer;
using Main.Infrastructure.Models.Soccer;
using Newtonsoft.Json;

namespace Prediction.Predicting.Soccer.Services
{
    public class SoccerTeamLastStatsDataService : ISoccerTeamLastStatsDataService
    {
        private readonly ISoccerStatsRepositoryService _soccerStatsRepositoryService;

        public SoccerTeamLastStatsDataService(ISoccerStatsRepositoryService soccerStatsRepositoryService)
        {
            _soccerStatsRepositoryService = soccerStatsRepositoryService;
        }

        public IEnumerable<SoccerStatsModel> GetTeamLastStats(string teamName, int lastMatchesStatsCount)
        {
            var teamLastStats = _soccerStatsRepositoryService
                 .GetAll(entity => entity.HostsTeam == teamName || entity.GuestsTeam == teamName)
                 .OrderByDescending(e => e.EventDate).Take(lastMatchesStatsCount).ToList();

            var resultStats = new List<SoccerStatsModel>();

            foreach (var soccerMatchStatsEntity in teamLastStats)
            {
                var stats = JsonConvert.DeserializeObject<IEnumerable<SoccerMatchStatsRow>>(soccerMatchStatsEntity.StatsJson);

                var teamStats = Enumerable.Empty<SoccerTeamStatsRowModel>();

                if (soccerMatchStatsEntity.HostsTeam == teamName)
                {
                    teamStats = stats.Select(s => new SoccerTeamStatsRowModel { StatsName = s.StatsName, StatsValue = s.HostsStatsValue }).ToList();
                }
                else
                {
                    teamStats = stats.Select(s => new SoccerTeamStatsRowModel { StatsName = s.StatsName, StatsValue = s.GuestsStatsValue }).ToList();
                }

                resultStats.Add(new SoccerStatsModel(soccerMatchStatsEntity.Result, soccerMatchStatsEntity.HostsTeam == teamName, teamStats));
            }

            return resultStats;
        }
    }
}
