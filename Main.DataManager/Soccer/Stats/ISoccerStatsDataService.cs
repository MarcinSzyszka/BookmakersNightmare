using System;
using System.Threading.Tasks;

namespace Main.DataManager.Soccer.Stats
{
    public interface ISoccerStatsDataService
    {
        Task UpdateResultsData(string country, string leagueName, DateTime? afterDate = null);
    }
}