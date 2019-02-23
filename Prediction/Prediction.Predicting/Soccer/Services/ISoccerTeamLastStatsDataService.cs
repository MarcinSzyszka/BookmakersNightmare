using System.Collections.Generic;
using Main.Infrastructure.Models.Soccer;

namespace Prediction.Predicting.Soccer.Services
{
    public interface ISoccerTeamLastStatsDataService
    {
        IEnumerable<SoccerStatsModel> GetTeamLastStats(string teamName, int lastMatchesStatsCount);
    }
}