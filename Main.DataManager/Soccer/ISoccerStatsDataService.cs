using System;
using System.Threading.Tasks;

namespace Main.DataManager.Soccer
{
    public interface ISoccerStatsDataService
    {
        Task UpdateResultsData(string country, string leagueName, DateTime? afterDate = null);
    }
}